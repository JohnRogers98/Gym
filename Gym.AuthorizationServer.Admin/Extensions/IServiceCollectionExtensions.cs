using Gym.AuthorizationServer.Admin.HostedServices;
using Gym.AuthorizationServer.Admin.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Gym.AuthorizationServer.Client;

namespace Gym.AuthorizationServer.Admin.Extensions;

public static class IServiceCollectionExtensions
{
    extension(IServiceCollection services) 
    {
        public IServiceCollection AddAuthenticationSchemes(String validIssuer, String validAudience)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = validIssuer,
                        ValidateAudience = true,
                        ValidAudience = validAudience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        ValidTypes = ["at+JWT"]
                    };
                    options.MapInboundClaims = false;

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = async (context) =>
                        {
                            var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
                            if (String.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                            {
                                return;
                            }

                            var token = authHeader.Substring("Bearer ".Length).Trim();
                            if (String.IsNullOrEmpty(token))
                            {
                                return;
                            }
                            context.Token = token;

                            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                            var kid = jwtToken.Header.Kid;

                            var rsaKeyProvider = context.HttpContext.RequestServices.GetRequiredService<IRsaSecurityKeyProvider>();

                            var signingKey = await rsaKeyProvider.GetKeyAsync(kid);
                            context.Options.TokenValidationParameters.IssuerSigningKey = signingKey;
                        }
                    };
                });

            return services;
        }

        public IServiceCollection AddAuthorizationPolicies()
        {
            services.AddAuthorizationBuilder()
                .AddPolicy(nameof(SecurityPolicy.AuthenticatedOnly), policy => policy.RequireAuthenticatedUser())
                .AddPolicy(nameof(SecurityPolicy.AdminOnly), policy => policy.RequireRole("Admin"))
                .AddPolicy(nameof(SecurityPolicy.ClientOnly), policy => policy.RequireRole("Client"))
                .AddPolicy(nameof(SecurityPolicy.InstructorOnly), policy => policy.RequireRole("Instructor"));

            return services;
        }

        public IServiceCollection AddAuthorizationServerClient(IConfiguration configuration)
        {
            var key = configuration.GetRequiredConfiguration("AuthorizationServer:ClientName");
            var baseUrl = configuration.GetRequiredConfiguration("AuthorizationServer:BaseUrl");
            services.AddHttpClient(key, client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                UseCookies = false,
                UseDefaultCredentials = false
            });

            services.SetupOAuthProtectedResourceConfiguration(options =>
            {
                options.ClientName = configuration.GetRequiredConfiguration("AuthorizationServer:ClientName");
                options.BaseUrl = configuration.GetRequiredConfiguration("AuthorizationServer:BaseUrl");
                options.Kid = configuration.GetRequiredConfiguration("AuthorizationServer:Kid");
            });

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

        public IServiceCollection AddServices()
        {
            services.AddSingleton<IRsaSecurityKeyProvider, RsaSecurityKeyProvider>();

            return services;
        }

    }
}
