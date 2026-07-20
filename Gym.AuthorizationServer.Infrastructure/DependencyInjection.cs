using Gym.AuthorizationServer.Infrastructure;
using Gym.AuthorizationServer.Infrastructure.Entities.AccessTokens;
using Gym.AuthorizationServer.Infrastructure.Entities.Clients;
using Gym.AuthorizationServer.Infrastructure.Entities.GrantCodes;
using Gym.AuthorizationServer.Infrastructure.Entities.ProtectedResources;
using Gym.AuthorizationServer.Infrastructure.Entities.RefreshTokens;
using Gym.AuthorizationServer.Infrastructure.Entities.Roles;
using Gym.AuthorizationServer.Infrastructure.Entities.Scopes;
using Gym.AuthorizationServer.Infrastructure.Entities.UserConsents;
using Gym.AuthorizationServer.Infrastructure.Entities.Users;
using Gym.AuthorizationServer.Infrastructure.Entities.Users.FormCredentials;
using Gym.AuthorizationServer.Infrastructure.Entities.Users.TelegramCredentials;
using Gym.AuthorizationServer.Infrastructure.Services;
using Gym.AuthorizationServer.Infrastructure.Session;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddMongoInfrastructure(Action<MongoOptions> configureOptions)
        {
            services.TryAddSingleton<IMongoClient>(sp => 
            {
                var options = sp.GetRequiredService<IOptions<MongoOptions>>().Value;
                return new MongoClient(options.ConnectionString);
            });

            services.TryAddSingleton<IMongoDatabase>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<MongoOptions>>().Value;
                return sp.GetRequiredService<IMongoClient>().GetDatabase(options.DatabaseName);
            });

            services.TryAddScoped<MongoUnitOfWork>();
            services.TryAddScoped<IUnitOfWork>(sp => sp.GetRequiredService<MongoUnitOfWork>());

            services
                .AddMongoCollection<UserEntity>(options => options.Collections.Users)
                .AddMongoCollection<FormCredentialEntity>(options => options.Collections.FormCredentials)
                .AddMongoCollection<TelegramCredentialEntity>(options => options.Collections.TelegramCredentials)
                .AddMongoCollection<ClientEntity>(options => options.Collections.Clients)
                .AddMongoCollection<UserConsentEntity>(options => options.Collections.UserConsents)
                .AddMongoCollection<GrantCodeEntity>(options => options.Collections.GrantCodes)
                .AddMongoCollection<AccessTokenEntity>(options => options.Collections.AccessTokens)
                .AddMongoCollection<RefreshTokenEntity>(options => options.Collections.RefreshTokens)
                .AddMongoCollection<UserRoleEntity>(options => options.Collections.Roles)
                .AddMongoCollection<ScopeEntity>(options => options.Collections.Scopes)
                .AddMongoCollection<ProtectedResourceEntity>(options => options.Collections.ProtectedResources);

            services.AddRepositories();

            services.AddPasswordHashingServices();

            return services;
        }

        private IServiceCollection AddMongoCollection<T>(Func<MongoOptions, String> collectionNameFunc)
        {
            services.TryAddSingleton<IMongoCollection<T>>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<MongoOptions>>().Value;
                var collectionName = collectionNameFunc(options);

                var database = sp.GetRequiredService<IMongoDatabase>();
                return database.GetCollection<T>(collectionName);
            });

            return services;
        }

        private IServiceCollection AddRepositories()
        {
            services.TryAddScoped<IUserRepository, UserRepository>();
            services.TryAddScoped<IFormCredentialRepository, FormCredentialRepository>();
            services.TryAddScoped<ITelegramCredentialRepository, TelegramCredentialRepository>();
            services.TryAddScoped<IUserConsentRepository, UserConsentRepository>();
            services.TryAddScoped<IRoleRepository, UserRoleRepository>();

            services.TryAddScoped<IClientRepository, ClientRepository>();
            services.TryAddScoped<IProtectedResourceRepository, ProtectedResourceRepository>();
            services.TryAddScoped<IScopeRepository, ScopeRepository>();
            services.TryAddScoped<IGrantCodeRepository, GrantCodeRepository>();
            services.TryAddScoped<IAccessTokenRepository, AccessTokenRepository>();
            services.TryAddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            return services;
        }

        private IServiceCollection AddPasswordHashingServices()
        {
            services.TryAddSingleton<IPasswordHasher, PasswordHasher>();
            services.TryAddSingleton<IPasswordHashValidator, PasswordHashValidator>();

            return services;
        }
    }
}
