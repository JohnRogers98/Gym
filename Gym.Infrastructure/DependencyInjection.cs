using Gym.Domain.CalendarEventAggregate;
using Gym.Domain.InstructorAggregate;
using Gym.Domain.TrainingAggregate;
using Gym.Domain.UserAggregate;
using Gym.Domain.UserAggregate.Authentication;
using Gym.Infrastructure.Configurations;
using Gym.Infrastructure.Entities.Repositories.Instructors;
using Gym.Infrastructure.Entities.Repositories.Trainings;
using Gym.Infrastructure.Entities.Repositories.Users;
using Gym.Infrastructure.Telegram;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoConsoleApp.Repositories.CalendarEvents;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Runtime.CompilerServices;

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
            
            services.AddMongoInfrastructure(mongoDbOptions ?? MongoDbOptions.Default);
            services.AddRepositories();
            services.AddQueryServices();

            if (configuration["TG_BOT_TOKEN"] is not null)
            {
                services.AddTelegramInfrastructure(configuration["TG_BOT_TOKEN"]!);
            }
            
            return services;
        }

        private static IServiceCollection AddMongoInfrastructure(this IServiceCollection services, MongoDbOptions mongoDbOptions)
        {
            services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoDbOptions.ConnectionString));
            services.AddSingleton<IMongoDatabase>(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDbOptions.DatabaseName));

            services.AddMongoCollection<InstructorEntity>(mongoDbOptions.CollectionOptions.Instructors);
            services.AddMongoCollection<TrainingEntity>(mongoDbOptions.CollectionOptions.Trainings);
            services.AddMongoCollection<CalendarEventEntity>(mongoDbOptions.CollectionOptions.CalendarEvents);
            services.AddMongoCollection<UserEntity>(mongoDbOptions.CollectionOptions.Users);

            return services;
        }

        private static IServiceCollection AddMongoCollection<T>(this IServiceCollection services, String collectionName)
        {
            services.AddSingleton<IMongoCollection<T>>(sp =>
            {
                var database = sp.GetRequiredService<IMongoDatabase>();
                return database.GetCollection<T>(collectionName);
            });

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.TryAddSingleton<IInstructorRepository, InstructorRepository>();
            services.TryAddSingleton<ITrainingRepository, TrainingRepository>();
            services.TryAddSingleton<ICalendarEventRepository, CalendarEventRepository>();
            services.TryAddSingleton<IUserRepository, UserRepository>();
            return services;
        }

        private static IServiceCollection AddQueryServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IInstructorQueryService, InstructorRepository>();
            services.TryAddSingleton<ITrainingQueryService, TrainingRepository>();
            services.TryAddSingleton<ICalendarEventQueryService, CalendarEventRepository>();
            services.TryAddSingleton<IUserQueryService, UserRepository>();
            return services;
        }

        private static IServiceCollection AddTelegramInfrastructure(this IServiceCollection services, String botToken)
        {
            services.AddSingleton<TelegramBotToken>(_ => TelegramBotToken.From(botToken));
            services.AddSingleton<ITelegramSignatureVerifier, TelegramSignatureVerifier>();
            return services;
        }
    }
}
