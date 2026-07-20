using Gym.Abstractions.Query.CalendarEvents;
using Gym.Abstractions.Query.Clients;
using Gym.Abstractions.Query.EventStore;
using Gym.Abstractions.Query.Instructors;
using Gym.Abstractions.Query.PersonalTrainings;
using Gym.Abstractions.Query.Trainings;
using Gym.Domain._Common;
using Gym.Domain.AccountContext;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.ClientContext;
using Gym.Domain.InstructorContext;
using Gym.Domain.PersonalTrainingContext;
using Gym.Domain.PollContext;
using Gym.Domain.PollResponseContext;
using Gym.Domain.TrainingContext;
using Gym.Domain.UserContext;
using Gym.Infrastructure.Caching;
using Gym.Infrastructure.Configurations;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.Deserializers;
using Gym.Infrastructure.Entities.EventStores.DtoDeserializers;
using Gym.Infrastructure.Entities.EventStores.Readers;
using Gym.Infrastructure.Entities.EventStores.Serializers;
using Gym.Infrastructure.Entities.Outbox;
using Gym.Infrastructure.Entities.Outbox.Readers;
using Gym.Infrastructure.Entities.Outbox.Updaters;
using Gym.Infrastructure.Entities.Projections.CalendarEvents;
using Gym.Infrastructure.Entities.Projections.Clients;
using Gym.Infrastructure.Entities.Projections.Events;
using Gym.Infrastructure.Entities.Projections.Instructors;
using Gym.Infrastructure.Entities.Projections.PersonalTrainings;
using Gym.Infrastructure.Entities.Projections.Trainings;
using Gym.Infrastructure.Entities.Repositories.Accounts;
using Gym.Infrastructure.Entities.Repositories.CalendarEvents;
using Gym.Infrastructure.Entities.Repositories.Clients;
using Gym.Infrastructure.Entities.Repositories.Instructors;
using Gym.Infrastructure.Entities.Repositories.PersonalTrainings;
using Gym.Infrastructure.Entities.Repositories.PollResponses;
using Gym.Infrastructure.Entities.Repositories.Polls;
using Gym.Infrastructure.Entities.Repositories.Trainings;
using Gym.Infrastructure.Entities.Repositories.Users;
using Gym.Infrastructure.HostedServices;
using Gym.Infrastructure.Scanners;
using Gym.Infrastructure.Telegram;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Net;
using System.Runtime.CompilerServices;
using Telegram.Bot;

[assembly: InternalsVisibleTo("Gym.Infrastructure.Tests")]

namespace Gym.Infrastructure;

public static class DependencyInjection
{
    static DependencyInjection()
    {
        var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);
    }

    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            services.AddOptions<MongoDbOptions>()
                .Bind(configuration.GetSection("MongodDb"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<ProxyOptions>()
                .Bind(configuration)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddMongoInfrastructure();
            services.AddMessagePublisher();
            services.AddRepositories();
            services.AddProjections();
            services.AddFinderServices();
            services.AddEventStore();
            services.AddCaching();

            services.AddHostedServices();

            services.AddMessageBusInfstrastructure(
                configuration.GetRequiredConfiguration("RabbitMQ:Hostname"),
                configuration.GetRequiredConfiguration("RabbitMQ:Username"),
                configuration.GetRequiredConfiguration("RabbitMQ:Password"),
                configuration.GetRequiredConfiguration("RabbitMQ:Vhost")
            );

            if (configuration["TG_BOT_TOKEN"] is not null)
            {
                services.AddTelegramInfrastructure(configuration["TG_BOT_TOKEN"]!);
            }

            return services;
        }

        private IServiceCollection AddMongoInfrastructure()
        {
            services.TryAddSingleton<IMongoClient>(sp => 
            {
                var options = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;
                return new MongoClient(options.ConnectionString);
            });

            services.TryAddSingleton<IMongoDatabase>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;
                var mongocClient = sp.GetRequiredService<IMongoClient>();
                return mongocClient.GetDatabase(options.DatabaseName);
            });

            services.TryAddScoped<MongoUnitOfWork>();
            services.TryAddScoped<IUnitOfWork>(sp => sp.GetRequiredService<MongoUnitOfWork>());

            services.AddMongoCollection<InstructorEntity>(options => options.Collections.Instructors);
            services.AddMongoCollection<InstructorProjection>(options => options.Collections.InstructorProjections);

            services.AddMongoCollection<TrainingEntity>(options => options.Collections.Trainings);
            services.AddMongoCollection<TrainingProjection>(options => options.Collections.TrainingProjections);

            services.AddMongoCollection<CalendarEventEntity>(options => options.Collections.CalendarEvents);
            services.AddMongoCollection<CalendarEventProjection>(options => options.Collections.CalendarEventProjections);

            services.AddMongoCollection<UserEntity>(options => options.Collections.Users);

            services.AddMongoCollection<ClientEntity>(options => options.Collections.Clients);
            services.AddMongoCollection<ClientProjection>(options => options.Collections.ClientProjections);

            services.AddMongoCollection<EventEntity>(options => options.Collections.Events);
            services.AddMongoCollection<EventProjection>(options => options.Collections.EventProjections);
            services.AddMongoCollection<MessageEntity>(options => options.Collections.Messages);

            services.AddMongoCollection<OutboxChangeStreamState>(options => options.Collections.OutboxChangeStreams);

            services.AddMongoCollection<PollEntity>(options => options.Collections.Polls);
            services.AddMongoCollection<PollResponseEntity>(options => options.Collections.PollResponses);

            services.AddMongoCollection<PersonalTrainingEntity>(options => options.Collections.PersonalTrainings);
            services.AddMongoCollection<PersonalTrainingProjection>(options => options.Collections.PersonalTrainingProjections);

            return services;
        }
        private IServiceCollection AddMongoCollection<T>(Func<MongoDbOptions, String> collectionNameFunc)
        {
            services.TryAddSingleton<IMongoCollection<T>>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;
                var collectionName = collectionNameFunc(options);

                var database = sp.GetRequiredService<IMongoDatabase>();
                return database.GetCollection<T>(collectionName);
            });

            return services;
        }

        private IServiceCollection AddRepositories()
        {
            services.TryAddScoped<IInstructorRepository, InstructorRepository>();
            services.TryDecorate<IInstructorRepository, InstructorEventStoreAspect>();

            services.TryAddScoped<ITrainingRepository, TrainingRepository>();
            services.TryDecorate<ITrainingRepository, TrainingEventStoreAspect>();

            services.TryAddScoped<ICalendarEventRepository, CalendarEventRepository>();
            services.TryDecorate<ICalendarEventRepository, CalendarEventEventStoreAspect>();

            services.TryAddScoped<IUserRepository, UserRepository>();

            services.TryAddScoped<IClientRepository, ClientRepository>();
            services.TryDecorate<IClientRepository, ClientEventStoreAspect>();

            services.TryAddScoped<IAccountRepository, AccountRepository>();

            services.TryAddScoped<IPollRepository, PollRepository>();
            services.TryDecorate<IPollRepository, PollEventStoreAspect>();

            services.TryAddScoped<IPollResponseRepository, PollResponseRepository>();
            services.TryDecorate<IPollResponseRepository, PollResponseEventStoreAspect>();

            services.TryAddScoped<IPersonalTrainingRepository, PersonalTrainingRepository>();
            services.TryDecorate<IPersonalTrainingRepository, PersonalTrainingEventStoreAspect>();

            return services;
        }

        private IServiceCollection AddMessagePublisher()
        {
            services.TryAddScoped<IMessagePublisher, OutboxStore>();
            services.TryAddSingleton<IOutboxResumeTokenStore, OutboxResumeTokenStore>();
            services.TryAddSingleton<IOutboxReader, OutboxReader>();
            services.TryAddScoped<IOutboxMessageStatusUpdater, OutboxMessageStatusUpdater>();
            services.TryAddScoped<IEventStoreReader, EventStoreReader>();
            return services;
        }

        private IServiceCollection AddProjections()
        {
            services.AddProjectionHanlders();

            services.TryAddScoped<EventProjectionStore>();
            services.TryAddScoped<IEventProjectionQueryService, EventProjectionQueryService>();

            services.TryAddScoped<ICalendarEventProjectionQueryService, CalendarEventProjectionQueryService>();
            services.TryAddScoped<IInstructorProjectionQueryService, InstructorProjectionQueryService>();
            services.TryAddScoped<ITrainingProjectionQueryService, TrainingProjectionQueryService>();
            services.TryAddScoped<IClientProjectionQueryService, ClientProjectionQueryService>();
            services.TryAddScoped<IPersonalTrainingProjectionQueryService, PersonalTrainingProjectionQueryService>();

            return services;
        }

        private IServiceCollection AddFinderServices()
        {
            services.TryAddScoped<IClientByUserIdFinder, ClientRepository>();
            services.TryAddScoped<IPastCalendarEventsFinder, CalendarEventRepository>();
            return services;
        }

        private IServiceCollection AddEventStore()
        {
            services.TryAddScoped<IEventStore, EventStore>();
            services.TryDecorate<IEventStore, OutboxEventStoreAspect>();

            var eventContractScanner = EventContractScanner.ScanAssembly(
                assembly: typeof(DependencyInjection).Assembly,
                serializer: typeof(EventSerializer),
                deserializer: typeof(EventDeserializer));

            services.TryAddSingleton<EventContractScanner>(eventContractScanner);

            services.TryAddSingleton<IEventSerializer, EventSerializer>();
            services.TryAddSingleton<IEventDeserializer, EventDeserializer>();
            services.TryAddSingleton<IEventDtoDeserializer, EventDtoDeserializer>();

            return services;
        }

        private IServiceCollection AddTelegramInfrastructure(String botToken)
        {
            services.TryAddSingleton<TelegramBotToken>(_ => TelegramBotToken.From(botToken));
            services.TryAddScoped<INotificationService, TelegramBotNotificationService>();

            services.TryAddSingleton<ITelegramBotClient>(sp =>
            {
                ProxyOptions proxyOptions = sp.GetRequiredService<IOptions<ProxyOptions>>().Value;

                WebProxy proxy = new WebProxy(proxyOptions.Host, Int32.Parse(proxyOptions.Port));
                proxy.Credentials = new NetworkCredential(proxyOptions.Login, proxyOptions.Password);
                var httpHandler = new SocketsHttpHandler
                {
                    Proxy = proxy,
                    UseProxy = true,
                    PooledConnectionLifetime = TimeSpan.FromMinutes(5)
                };

                var httpProxyClient = new HttpClient(httpHandler);

                return new TelegramBotClient(botToken, httpClient: httpProxyClient, cancellationToken: CancellationToken.None);
            });

            return services;
        }

        private IServiceCollection AddCaching()
        {
            services.TryAddScoped<IExclusiveAccessCoordinator, MemoryCacheExclusiveAccess>();
            return services;
        }

        private IServiceCollection AddHostedServices()
        {
            services.AddHostedService<ConfigurationLogger>();

            services.AddHostedService<OutboxEventReader>();

            services.TryAddKeyedSingleton<PeriodicTimer>(
                nameof(CalendarEventCompletionChecker),
                (_, _) => new PeriodicTimer(TimeSpan.FromMinutes(5), TimeProvider.System)
            );
            services.AddHostedService<CalendarEventCompletionChecker>();

            services.TryAddKeyedSingleton<PeriodicTimer>(
                nameof(OutboxDeadLetterRevoker),
                (_, _) => new PeriodicTimer(TimeSpan.FromMinutes(1), TimeProvider.System)
            );
            services.AddHostedService<OutboxDeadLetterRevoker>();

            return services;
        }

        private IServiceCollection AddMessageBusInfstrastructure(String hostname, String username, String password, String vhost)
        {
            services.AddRabbitMQConnection(options =>
            {
                options.Hostname = hostname;
                options.Username = username;
                options.Password = password;
                options.Vhost = vhost;
            });

            services.AddHostedService<MessageBusInitializer>();
            services.AddHostedService<UserCreatedMessagesConsumer>();

            return services;
        }
    }

}
