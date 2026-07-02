using Gym.AuthorizationServer.Admin.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

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

        public IServiceCollection AddAuthorizationServerNamedClient(String key, String baseUrl)
        {
            services.AddHttpClient(key, client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                UseCookies = false,
                UseDefaultCredentials = false
            });

            return services;
        }

        public IServiceCollection AddServices()
        {
            services.AddSingleton<IRsaSecurityKeyProvider, RsaSecurityKeyProvider>();

            return services;
        }

    }
}
