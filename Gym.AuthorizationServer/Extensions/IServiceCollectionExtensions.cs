using Gym.AuthorizationServer.HostedServices;
using Gym.AuthorizationServer.Infrastructure.Entities.Users;
using Gym.AuthorizationServer.Options;
using Gym.AuthorizationServer.Services;
using Gym.AuthorizationServer.Services.Flows;
using Gym.AuthorizationServer.Services.Rsa;
using Gym.AuthorizationServer.Services.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace Gym.AuthorizationServer.Extensions;

public static class IServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddConfigurationOptions(IConfiguration configuration)
        {
            services.AddOptions<JwtOptions>()
              .Bind(configuration.GetRequiredSection(JwtOptions.SectionName))
              .ValidateDataAnnotations()
              .ValidateOnStart();

            return services;
        }

        public IServiceCollection AddCorsPolicy(String policyName, params String[] origins)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(policyName, policy =>
                {
                    policy.WithOrigins(origins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            return services;
        }

        public IServiceCollection AddAuthenticationSchemes()
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "__Host-Gym.AuthorizationServer.Auth";
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        ValidTypes = [AccessTokenGenerator.TypHeader]
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

        public IServiceCollection AddServices()
        {
            services.TryAddSingleton<IAccessTokenGenerator, AccessTokenGenerator>();
            services.TryAddSingleton<IRandomBase64StringGenerator, RandomBase64StringGenerator>();
            services.TryAddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.TryAddSingleton<IRequestIdGenerator, RequestIdGenerator>();
            services.TryAddSingleton<IGrantCodeGenerator, GrantCodeGenerator>();
            services.TryAddSingleton<IScopeChecker, ScopeChecker>();
            services.TryAddScoped<IScopeGrantResolveService, ScopeGrantResolveService>();
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

            services.TryAddScoped<IUserByUsernameAndPasswordFinder, UserByUsernameAndPasswordFinder>();
            services.TryAddScoped<IUserRoleByUserIdFinder, UserRoleByUserIdFinder>();

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

        public IServiceCollection AddDatabaseInfrastructure(IConfiguration configuration)
        {
            services.AddMongoInfrastructure(options =>
            {
                options.ConnectionString = configuration.GetRequiredConfiguration("MongoDb:ConnectionString");
                options.DatabaseName = configuration.GetRequiredConfiguration("MongoDb:DatabaseName");
            });

            return services;
        }

        public IServiceCollection AddMessageBusInfstrastructure(IConfiguration configuration)
        {
            services.AddRabbitMQConnection(options =>
            {
                options.Hostname = configuration.GetRequiredConfiguration("RabbitMQ:Hostname");
                options.Username = configuration.GetRequiredConfiguration("RabbitMQ:Username");
                options.Password = configuration.GetRequiredConfiguration("RabbitMQ:Password");
                options.Vhost = configuration.GetRequiredConfiguration("RabbitMQ:Vhost");
            });

            services.AddHostedService<MessageBusInitializer>();

            return services;
        }
    }
}
