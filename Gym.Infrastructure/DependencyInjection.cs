using Gym.Abstractions.Query.CalendarEvents;
using Gym.Abstractions.Query.Clients;
using Gym.Abstractions.Query.EventStore;
using Gym.Abstractions.Query.Instructors;
using Gym.Abstractions.Query.Trainings;
using Gym.Domain._Common;
using Gym.Domain.AccountContext;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.ClientContext;
using Gym.Domain.FormAuthContext;
using Gym.Domain.InstructorContext;
using Gym.Domain.PollContext;
using Gym.Domain.PollResponseContext;
using Gym.Domain.TelegramAuthContext;
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
using Gym.Infrastructure.Entities.Projections.Trainings;
using Gym.Infrastructure.Entities.Repositories.Accounts;
using Gym.Infrastructure.Entities.Repositories.CalendarEvents;
using Gym.Infrastructure.Entities.Repositories.Clients;
using Gym.Infrastructure.Entities.Repositories.FormAuths;
using Gym.Infrastructure.Entities.Repositories.Instructors;
using Gym.Infrastructure.Entities.Repositories.PollResponses;
using Gym.Infrastructure.Entities.Repositories.Polls;
using Gym.Infrastructure.Entities.Repositories.TelgramAuths;
using Gym.Infrastructure.Entities.Repositories.Trainings;
using Gym.Infrastructure.Entities.Repositories.Users;
using Gym.Infrastructure.HostedServices;
using Gym.Infrastructure.Scanners;
using Gym.Infrastructure.Telegram;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Runtime.CompilerServices;
using Telegram.Bot;

[assembly: InternalsVisibleTo("Gym.Infrastructure.Tests")]

namespace Gym.Infrastructure
{
    public static class DependencyInjection
    {
        static DependencyInjection()
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            MongoDbOptions mongoDbOptions = configuration.GetSection("MongodDb").Get<MongoDbOptions>() ?? MongoDbOptions.Default;
            services.TryAddSingleton<MongoDbOptions>(_ => mongoDbOptions);

            services.AddMongoInfrastructure(mongoDbOptions ?? MongoDbOptions.Default);
            services.AddMessagePublisher();
            services.AddRepositories();
            services.AddProjections();
            services.AddFinderServices();
            services.AddEventStore();
            services.AddCaching();
            services.AddPasswordServices();

            services.AddBackgroundWorkers();

            if (configuration["TG_BOT_TOKEN"] is not null)
            {
                services.AddTelegramInfrastructure(configuration["TG_BOT_TOKEN"]!);
            }
            
            return services;
        }

        private static IServiceCollection AddMongoInfrastructure(this IServiceCollection services, MongoDbOptions mongoDbOptions)
        {
            services.TryAddSingleton<IMongoClient>(_ => new MongoClient(mongoDbOptions.ConnectionString));
            services.TryAddSingleton<IMongoDatabase>(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDbOptions.DatabaseName));

            services.TryAddScoped<MongoUnitOfWork>();
            services.TryAddScoped<IUnitOfWork>(sp => sp.GetRequiredService<MongoUnitOfWork>());

            services.AddMongoCollection<InstructorEntity>(mongoDbOptions.CollectionOptions.Instructors);
            services.AddMongoCollection<InstructorProjection>(mongoDbOptions.CollectionOptions.InstructorProjections);

            services.AddMongoCollection<TrainingEntity>(mongoDbOptions.CollectionOptions.Trainings);
            services.AddMongoCollection<TrainingProjection>(mongoDbOptions.CollectionOptions.TrainingProjections);

            services.AddMongoCollection<CalendarEventEntity>(mongoDbOptions.CollectionOptions.CalendarEvents);
            services.AddMongoCollection<CalendarEventProjection>(mongoDbOptions.CollectionOptions.CalendarEventProjections);

            services.AddMongoCollection<UserEntity>(mongoDbOptions.CollectionOptions.Users);

            services.AddMongoCollection<TelegramAuthEntity>(mongoDbOptions.CollectionOptions.TelegramAuths);
            services.AddMongoCollection<FormAuthEntity>(mongoDbOptions.CollectionOptions.FormAuths);

            services.AddMongoCollection<ClientEntity>(mongoDbOptions.CollectionOptions.Clients);
            services.AddMongoCollection<ClientProjection>(mongoDbOptions.CollectionOptions.ClientProjections);

            services.AddMongoCollection<EventEntity>(mongoDbOptions.CollectionOptions.Events);
            services.AddMongoCollection<EventProjection>(mongoDbOptions.CollectionOptions.EventProjections);
            services.AddMongoCollection<MessageEntity>(mongoDbOptions.CollectionOptions.Messages);

            services.AddMongoCollection<OutboxChangeStreamState>(mongoDbOptions.CollectionOptions.OutboxChangeStreams);
            
            services.AddMongoCollection<PollEntity>(mongoDbOptions.CollectionOptions.Polls);
            services.AddMongoCollection<PollResponseEntity>(mongoDbOptions.CollectionOptions.PollResponses);

            return services;
        }

        private static IServiceCollection AddMongoCollection<T>(this IServiceCollection services, String collectionName)
        {
            services.TryAddSingleton<IMongoCollection<T>>(sp =>
            {
                var database = sp.GetRequiredService<IMongoDatabase>();
                return database.GetCollection<T>(collectionName);
            });

            return services;
        }

        private static IServiceCollection AddMessagePublisher(this IServiceCollection services)
        {
            services.TryAddScoped<IMessagePublisher, OutboxStore>();
            services.TryAddSingleton<IOutboxResumeTokenStore, OutboxResumeTokenStore>();
            services.TryAddSingleton<IOutboxReader, OutboxReader>();
            services.TryAddScoped<IOutboxMessageStatusUpdater, OutboxMessageStatusUpdater>();
            services.TryAddScoped<IEventStoreReader, EventStoreReader>();
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.TryAddScoped<IInstructorRepository, InstructorRepository>();
            services.TryDecorate<IInstructorRepository, InstructorEventStoreAspect>();

            services.TryAddScoped<ITrainingRepository, TrainingRepository>();
            services.TryDecorate<ITrainingRepository, TrainingEventStoreAspect>();

            services.TryAddScoped<ICalendarEventRepository, CalendarEventRepository>();
            services.TryDecorate<ICalendarEventRepository, CalendarEventEventStoreAspect>();

            services.TryAddScoped<IUserRepository, UserRepository>();

            services.TryAddScoped<ITelegramAuthRepository, TelegramAuthRepository>();
            services.TryAddScoped<IFormAuthRepository, FormAuthRepository>();

            services.TryAddScoped<IClientRepository, ClientRepository>();
            services.TryDecorate<IClientRepository, ClientEventStoreAspect>();
            
            services.TryAddScoped<IAccountRepository, AccountRepository>();

            services.TryAddScoped<IPollRepository, PollRepository>();
            services.TryDecorate<IPollRepository, PollEventStoreAspect>();

            services.TryAddScoped<IPollResponseRepository, PollResponseRepository>();
            services.TryDecorate<IPollResponseRepository, PollResponseEventStoreAspect>();

            return services;
        }

        private static IServiceCollection AddProjections(this IServiceCollection services)
        {
            services.AddProjectionHanlders();

            services.TryAddScoped<EventProjectionStore>();
            services.TryAddScoped<IEventProjectionQueryService, EventProjectionQueryService>();

            services.TryAddScoped<ICalendarEventProjectionQueryService, CalendarEventProjectionQueryService>();
            services.TryAddScoped<IInstructorProjectionQueryService, InstructorProjectionQueryService>();
            services.TryAddScoped<ITrainingProjectionQueryService, TrainingProjectionQueryService>();
            services.TryAddScoped<IClientProjectionQueryService, ClientProjectionQueryService>();

            return services;
        }

        private static IServiceCollection AddFinderServices(this IServiceCollection services)
        {
            services.TryAddScoped<ITelegramAuthByUserIdFinder, TelegramAuthRepository>();
            services.TryAddScoped<IClientByUserIdFinder, ClientRepository>();
            services.TryAddScoped<IClientByUserIdFinder, ClientRepository>();
            services.TryAddScoped<IPastCalendarEventsFinder, CalendarEventRepository>();
            return services;
        }

        private static IServiceCollection AddEventStore(this IServiceCollection services)
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

        private static IServiceCollection AddTelegramInfrastructure(this IServiceCollection services, String botToken)
        {
            services.TryAddSingleton<TelegramBotToken>(_ => TelegramBotToken.From(botToken));
            services.TryAddSingleton<ITelegramSignatureVerifier, TelegramSignatureVerifier>();
            services.TryAddScoped<INotificationService, TelegramBotNotificationService>();

            services.TryAddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(botToken, cancellationToken: CancellationToken.None));

            return services;
        }

        private static IServiceCollection AddCaching(this IServiceCollection services)
        {
            services.TryAddScoped<IExclusiveAccessCoordinator, MemoryCacheExclusiveAccess>();
            return services;
        }

        private static IServiceCollection AddPasswordServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IPasswordHasher, PasswordHasher>();
            services.TryAddSingleton<IPasswordHashValidator, PasswordHashValidator>();
            services.TryAddSingleton<IPasswordGenerator, PasswordGenerator>();
            return services;
        }

        private static IServiceCollection AddBackgroundWorkers(this IServiceCollection services)
        {
            services.AddHostedService<OutboxReaderHostedService>();

            services.TryAddKeyedSingleton<PeriodicTimer>(
                nameof(CalendarEventCompletionChecker),
                (_, _) => new PeriodicTimer(TimeSpan.FromMinutes(5), TimeProvider.System)
            );
            services.AddHostedService<CalendarEventCompletionChecker>();
            return services;
        }
    }
}
