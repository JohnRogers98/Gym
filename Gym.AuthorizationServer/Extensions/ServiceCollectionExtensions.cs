using Gym.AuthorizationServer.Entities.AccessTokens;
using Gym.AuthorizationServer.Entities.Clients;
using Gym.AuthorizationServer.Entities.GrantCodes;
using Gym.AuthorizationServer.Entities.RefreshTokens;
using Gym.AuthorizationServer.Entities.UserConsents;
using Gym.AuthorizationServer.Entities.Users;
using Gym.AuthorizationServer.Entities.Users.FormCredentials;
using Gym.AuthorizationServer.Entities.Users.TelegramCredentials;
using Gym.AuthorizationServer.Services;
using Idp.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;

namespace Gym.AuthorizationServer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddRepositories()
            {
                services.TryAddScoped<IUserRepository, UserRepository>();
                services.TryAddScoped<IFormCredentialRepository, FormCredentialRepository>();
                services.TryAddScoped<ITelegramCredentialRepository, TelegramCredentialRepository>();
                services.TryAddScoped<IUserByUsernameAndPasswordFinder, UserByUsernameAndPasswordFinder>();
                services.TryAddScoped<IUserConsentRepository, UserConsentRepository>();

                services.TryAddScoped<IClientRepository, ClientRepository>();
                services.TryAddScoped<IGrantCodeRepository, GrantCodeRepository>();
                services.TryAddScoped<IAccessTokenRepository, AccessTokenRepository>();
                services.TryAddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

                return services;
            }

            public IServiceCollection AddServices()
            {
                services.TryAddSingleton<IAccessTokenGenerator, AccessTokenGenerator>();
                services.TryAddSingleton<IRandomStringGenerator, RandomStringGenerator>();
                services.TryAddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
                services.TryAddSingleton<IRequestIdGenerator, RequestIdGenerator>();
                services.TryAddSingleton<IGrantCodeGenerator, GrantCodeGenerator>();
                services.TryAddSingleton<IScopeChecker, ScopeChecker>();
                services.TryAddSingleton<IPasswordHasher, PasswordHasher>();
                services.TryAddSingleton<IPasswordHashValidator, PasswordHashValidator>();
                services.TryAddSingleton<IClientSecretHashValidator, ClientSecretHashValidator>();
                services.TryAddSingleton<ICodeChallangeVerifier, CodeChallangeVerifier>();
                services.TryAddSingleton<ITelegramSignatureVerifier, TelegramSignatureVerifier>();

                services.TryAddScoped<IUpsertUserConsentService, UpsertUserConsentService>();
                services.TryAddScoped<IConsentEvaluationService, ConsentEvaluationService>();

                services.TryAddScoped<ITokenFlowCoordinator, TokenFlowCoordinator>();
                services.TryAddScoped<IAuthorizationCodeFlowService, AuthorizationCodeFlowService>();
                services.TryAddScoped<IRefreshTokenFlowService, RefreshTokenFlowService>();
                services.TryAddScoped<ITelegramAssertionFlowService, TelegramAssertionFlowService>();

                return services;
            }

            public IServiceCollection AddTelegramBotToken(IConfiguration configuration)
            {
                var tgBotToken = configuration["TelegramBot:Token"]
                    ?? throw new InvalidOperationException("TelegramBot:Token is not configured");

                services.TryAddSingleton<TelegramBotToken>(_ => new TelegramBotToken(tgBotToken));

                return services;
            }

            public IServiceCollection AddRsaSigningService(IConfiguration configuration)
            {
                services.TryAddSingleton<IRsaSigningService, RsaSigningService>();
                return services;
            }

            public IServiceCollection AddMongoInfrastructure(IConfiguration configuration)
            {
                var mongoConnectionString = configuration["MongoDb:ConnectionString"]
                    ?? throw new InvalidOperationException("MongoDb:ConnectionString is not configured");

                services.TryAddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));
                services.TryAddSingleton<IMongoDatabase>(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(configuration["MongoDb:DatabaseName"]));

                /*services.TryAddScoped<MongoUnitOfWork>();
                services.TryAddScoped<IUnitOfWork>(sp => sp.GetRequiredService<MongoUnitOfWork>());*/

                services.AddMongoCollection<UserEntity>(configuration["MongoDb:Collections:Users"]!);
                services.AddMongoCollection<FormCredentialEntity>(configuration["MongoDb:Collections:FormCredentials"]!);
                services.AddMongoCollection<TelegramCredentialEntity>(configuration["MongoDb:Collections:TelegramCredentials"]!);
                services.AddMongoCollection<ClientEntity>(configuration["MongoDb:Collections:Clients"]!);
                services.AddMongoCollection<UserConsentEntity>(configuration["MongoDb:Collections:UserConsents"]!);
                services.AddMongoCollection<GrantCodeEntity>(configuration["MongoDb:Collections:GrantCodes"]!);
                services.AddMongoCollection<AccessTokenEntity>(configuration["MongoDb:Collections:AccessTokens"]!);
                services.AddMongoCollection<RefreshTokenEntity>(configuration["MongoDb:Collections:RefreshTokens"]!);

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
