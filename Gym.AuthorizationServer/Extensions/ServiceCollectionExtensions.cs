using Gym.AuthorizationServer.Infrastructure.Entities.AccessTokens;
using Gym.AuthorizationServer.Infrastructure.Entities.Clients;
using Gym.AuthorizationServer.Infrastructure.Entities.GrantCodes;
using Gym.AuthorizationServer.Infrastructure.Entities.RefreshTokens;
using Gym.AuthorizationServer.Infrastructure.Entities.UserConsents;
using Gym.AuthorizationServer.Infrastructure.Entities.Users;
using Gym.AuthorizationServer.Infrastructure.Entities.Users.FormCredentials;
using Gym.AuthorizationServer.Infrastructure.Entities.Users.TelegramCredentials;
using Gym.AuthorizationServer.Infrastructure.Session;
using Gym.AuthorizationServer.Services;
using Gym.AuthorizationServer.Services.Flows;
using Gym.AuthorizationServer.Services.Rsa;
using Gym.AuthorizationServer.Services.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace Gym.AuthorizationServer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddAuthenticationSchemes()
            {
                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie()
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            RequireExpirationTime = true
                        };
                        options.MapInboundClaims = false;

                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = async (context) =>
                            {
                                var rsaSecurityKeyProvider = context.HttpContext.RequestServices.GetRequiredService<IRsaSecurityKeyProvider>();
                                context.Options.TokenValidationParameters.IssuerSigningKey = rsaSecurityKeyProvider.GetRsaSecurityKey();
                            }
                        };
                    });

                return services;
            }

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
                services.TryAddSingleton<IRandomBase64StringGenerator, RandomBase64StringGenerator>();
                services.TryAddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
                services.TryAddSingleton<IRequestIdGenerator, RequestIdGenerator>();
                services.TryAddSingleton<IGrantCodeGenerator, GrantCodeGenerator>();
                services.TryAddSingleton<IScopeChecker, ScopeChecker>();
                services.TryAddSingleton<IPasswordHasher, PasswordHasher>();
                services.TryAddSingleton<IPasswordHashValidator, PasswordHashValidator>();
                services.TryAddSingleton<IClientSecretHashValidator, ClientSecretHashValidator>();
                services.TryAddSingleton<ICodeChallangeVerifier, CodeChallangeVerifier>();
                services.TryAddSingleton<ITelegramSignatureVerifier, TelegramSignatureVerifier>();

                services.TryAddSingleton<IComputeOpenIdAtHashService, ComputeOpenIdAtHashService>();
                services.TryAddSingleton<IIdTokenGenerator, IdTokenGenerator>();
                services.TryAddSingleton<IIdTokenGeneratorHelper, IdTokenGeneratorHelper>();

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
                services.TryAddSingleton<IRsaKeyProvider, RsaKeyProvider>();
                services.TryAddSingleton<IRsaSecurityKeyProvider, RsaSecurityKeyProvider>();
                services.TryAddSingleton<IRsaSigningCredentialsProvider, RsaSigningCredentialsProvider>();
                services.TryAddSingleton<IRsaJwkService, RsaJwkService>();
                return services;
            }

            public IServiceCollection AddMongoInfrastructure(IConfiguration configuration)
            {
                var mongoConnectionString = configuration["MongoDb:ConnectionString"]
                    ?? throw new InvalidOperationException("MongoDb:ConnectionString is not configured");

                services.TryAddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));
                services.TryAddSingleton<IMongoDatabase>(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(configuration["MongoDb:DatabaseName"]));

                services.TryAddScoped<MongoUnitOfWork>();
                services.TryAddScoped<IUnitOfWork>(sp => sp.GetRequiredService<MongoUnitOfWork>());

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
