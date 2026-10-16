using Gym.AuthorizationServer.Client;
using Gym.Chat.WSApi.Options;
using Gym.Redis.Client.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace Gym.Chat.WSApi.Extensions;

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
                    options.MapInboundClaims = true;

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = services.OnMessageReceivedEventHandlerAsync
                    };
                });

            return services;
        }
        private async Task OnMessageReceivedEventHandlerAsync(MessageReceivedContext context)
        {
            #region GetCookie
            var authenticationCookieOptions = context.HttpContext.RequestServices
                            .GetRequiredService<IOptions<AuthenticationCookieOptions>>();

            var encryptedClientSessionCookie = context.Request.Cookies[authenticationCookieOptions.Value.Name];
            if (String.IsNullOrEmpty(encryptedClientSessionCookie))
            {
                context.Fail("Client session cookie not exist");
                return;
            }
            #endregion

            try
            {
                #region Unprotect cookie
                var protector = context.HttpContext.RequestServices
                    .GetDataProtector(authenticationCookieOptions.Value.DataProtectorPurpose);

                var ticketDataFormat = new TicketDataFormat(protector);
                var ticket = ticketDataFormat.Unprotect(encryptedClientSessionCookie);
                if (ticket is null)
                {
                    context.Fail("Cannot unprotect ticket");
                    return;
                }
                #endregion

                #region Get AccessToken
                var clientSessionKey = ticket.Principal.Claims
                    .FirstOrDefault(c => c.Type == ExtendedClaimTypes.ClientSessionKey)?.Value;
                if (String.IsNullOrEmpty(clientSessionKey))
                {
                    context.Fail("Required client session key claim not found");
                    return;
                }

                var getSessionTokensService = context.HttpContext.RequestServices
                    .GetRequiredService<IGetSessionTokensService>();

                var tokens = await getSessionTokensService.HandleAsync(clientSessionKey);
                if (tokens?.AccessToken is null)
                {
                    context.Fail("Invalid token");
                    return;
                }

                context.Token = tokens.AccessToken;
                #endregion

                #region Get JWT public key
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(tokens.AccessToken);
                var kid = jwtToken.Header.Kid;

                var rsaKeyProvider = context.HttpContext.RequestServices.GetRequiredService<IRsaSecurityKeyProvider>();

                var signingKey = await rsaKeyProvider.GetKeyAsync(kid);
                context.Options.TokenValidationParameters.IssuerSigningKey = signingKey;
                #endregion
            }
            catch (CryptographicException)
            {
                context.Fail("Invalid token cookie");
            }
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

        public IServiceCollection AddCorsPolicy(String policyName, IConfiguration configuration)
        {
            var spaCorsOrigins = configuration.GetRequiredSection("CorsOrigins").Get<String[]>();
            if (spaCorsOrigins is null)
                throw new InvalidOperationException("No CorsOrigins presented in configurations.");

            services.AddCors(options =>
            {
                options.AddPolicy(policyName, policy =>
                {
                    policy.WithOrigins(spaCorsOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            return services;
        }

        public IServiceCollection AddOptionsFromConfiguration(IConfiguration configuration)
        {
            services.AddOptions<AuthenticationCookieOptions>()
                .Bind(configuration.GetRequiredSection(AuthenticationCookieOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

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

        public IServiceCollection AddRedisClient(IConfiguration configuration)
        {
            services.AddRedis(
                clientOptions =>
                {
                    clientOptions.ConnectionString = configuration.GetRequiredConfiguration("Redis:ConnectionString");
                }
            );

            services.AddSessionServices();

            return services;
        }

        public IServiceCollection AddServices()
        {
            services.AddSingleton<IRsaSecurityKeyProvider, RsaSecurityKeyProvider>();

            return services;
        }
    }
}
