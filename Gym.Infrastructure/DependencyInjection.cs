using Gym.Abstractions.Query.CalendarEvents;
using Gym.Abstractions.Query.EventStore;
using Gym.Domain._Common;
using Gym.Domain.AccountContext;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.ClientContext;
using Gym.Domain.InstructorContext;
using Gym.Domain.TrainingContext;
using Gym.Domain.UserContext;
using Gym.Domain.UserContext.Authentication;
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
using Gym.Infrastructure.Entities.Projections;
using Gym.Infrastructure.Entities.Projections.CalendarEvents;
using Gym.Infrastructure.Entities.Projections.Events;
using Gym.Infrastructure.Entities.Repositories.Accounts;
using Gym.Infrastructure.Entities.Repositories.CalendarEvents;
using Gym.Infrastructure.Entities.Repositories.Clients;
using Gym.Infrastructure.Entities.Repositories.Instructors;
using Gym.Infrastructure.Entities.Repositories.Trainings;
using Gym.Infrastructure.Entities.Repositories.Users;
using Gym.Infrastructure.HostedServices;
using Gym.Infrastructure.Telegram;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoConsoleApp.Repositories.CalendarEvents;
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
            services.AddQueryServices();
            services.AddEventStore();
            services.AddCaching();

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
            services.AddMongoCollection<TrainingEntity>(mongoDbOptions.CollectionOptions.Trainings);
            services.AddMongoCollection<CalendarEventEntity>(mongoDbOptions.CollectionOptions.CalendarEvents);
            services.AddMongoCollection<CalendarEventProjection>(mongoDbOptions.CollectionOptions.CalendarEventProjections);
            services.AddMongoCollection<UserEntity>(mongoDbOptions.CollectionOptions.Users);
            services.AddMongoCollection<ClientEntity>(mongoDbOptions.CollectionOptions.Clients);
            services.AddMongoCollection<EventEntity>(mongoDbOptions.CollectionOptions.Events);
            services.AddMongoCollection<MessageEntity>(mongoDbOptions.CollectionOptions.Messages);
            services.AddMongoCollection<EventProjection>(mongoDbOptions.CollectionOptions.EventProjections);
            services.AddMongoCollection<CalendarEventProjection>(mongoDbOptions.CollectionOptions.EventProjections);
            services.AddMongoCollection<OutboxChangeStreamState>(mongoDbOptions.CollectionOptions.OutboxChangeStreams);

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
            services.TryAddScoped<ITrainingRepository, TrainingRepository>();

            services.TryAddScoped<ICalendarEventRepository, CalendarEventRepository>();
            services.TryDecorate<ICalendarEventRepository, CalendarEventEventStoreAspect>();

            services.TryAddScoped<IUserRepository, UserRepository>();

            services.TryAddScoped<IClientRepository, ClientRepository>();
            services.TryDecorate<IClientRepository, ClientEventStoreAspect>();
            
            services.TryAddScoped<IAccountRepository, AccountRepository>();
            return services;
        }

        private static IServiceCollection AddProjections(this IServiceCollection services)
        {
            var factoryTypes = typeof(IProjectionHandler).Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract &&
                            !t.IsInterface &&
                            typeof(IProjectionHandler).IsAssignableFrom(t) &&
                            t != typeof(CompositeProjectionHandler))
                .ToList();

            foreach (var type in factoryTypes)
            {
                services.TryAddScoped(type);
            }

            services.TryAddScoped<IProjectionHandler>(sp =>
            {
                var handlers = new List<IProjectionHandler>();
                foreach (var type in factoryTypes)
                {
                    var handler = sp.GetRequiredService(type) as IProjectionHandler;
                    if (handler != null)
                    {
                        handlers.Add(handler);
                    }
                }

                return new CompositeProjectionHandler(handlers);
            });

            services.TryAddScoped<EventProjectionStore>();
            services.TryAddScoped<IEventProjectionQueryService, EventProjectionQueryService>();

            services.TryAddScoped<ICalendarEventProjectionQueryService, CalendarEventProjectionQueryService>();

            return services;
        }

        private static IServiceCollection AddQueryServices(this IServiceCollection services)
        {
            services.TryAddScoped<IInstructorQueryService, InstructorRepository>();
            services.TryAddScoped<ITrainingQueryService, TrainingRepository>();
            services.TryAddScoped<IUserByTelegramIdFinder, UserRepository>();
            services.TryAddScoped<IClientByUserIdFinder, ClientRepository>();
            return services;
        }

        private static IServiceCollection AddEventStore(this IServiceCollection services)
        {
            services.TryAddScoped<IEventStore, EventStore>();
            services.TryDecorate<IEventStore, OutboxEventStoreAspect>();

            services.TryAddScoped<IEventSerializer, EventSerializer>();
            services.TryAddScoped<IEventDeserializer, EventDeserializer>();
            services.TryAddScoped<IEventDtoDeserializer, EventDtoDeserializer>();

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

        private static IServiceCollection AddBackgroundWorkers(this IServiceCollection services)
        {
            services.AddHostedService<OutboxReaderHostedService>();
            return services;
        }
    }
}
