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
using MongoDB.Driver;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddRepositories()
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

            public IServiceCollection AddPasswordHashingServices()
            {
                services.TryAddSingleton<IPasswordHasher, PasswordHasher>();
                services.TryAddSingleton<IPasswordHashValidator, PasswordHashValidator>();

                return services;
            }

            public IServiceCollection AddMongoInfrastructure(MongoOptions mongoOptions)
            {
                services.TryAddSingleton<IMongoClient>(_ => new MongoClient(mongoOptions.ConnectionString));
                services.TryAddSingleton<IMongoDatabase>(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoOptions.DatabaseName));

                services.TryAddScoped<MongoUnitOfWork>();
                services.TryAddScoped<IUnitOfWork>(sp => sp.GetRequiredService<MongoUnitOfWork>());

                services.AddMongoCollection<UserEntity>(mongoOptions.Collections.Users);
                services.AddMongoCollection<FormCredentialEntity>(mongoOptions.Collections.FormCredentials);
                services.AddMongoCollection<TelegramCredentialEntity>(mongoOptions.Collections.TelegramCredentials);
                services.AddMongoCollection<ClientEntity>(mongoOptions.Collections.Clients);
                services.AddMongoCollection<UserConsentEntity>(mongoOptions.Collections.UserConsents);
                services.AddMongoCollection<GrantCodeEntity>(mongoOptions.Collections.GrantCodes);
                services.AddMongoCollection<AccessTokenEntity>(mongoOptions.Collections.AccessTokens);
                services.AddMongoCollection<RefreshTokenEntity>(mongoOptions.Collections.RefreshTokens);
                services.AddMongoCollection<UserRoleEntity>(mongoOptions.Collections.Roles);
                services.AddMongoCollection<ScopeEntity>(mongoOptions.Collections.Scopes);
                services.AddMongoCollection<ProtectedResourceEntity>(mongoOptions.Collections.ProtectedResources);

                return services;
            }

            private IServiceCollection AddMongoCollection<T>(String collectionName)
            {
                services.TryAddSingleton<IMongoCollection<T>>(sp =>
                {
                    var database = sp.GetRequiredService<IMongoDatabase>();
                    return database.GetCollection<T>(collectionName);
                });

                return services;
            }
        }
    }
}
