using Gym.AuthorizationServer.Admin.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Gym.AuthorizationServer.Admin.Extensions;

public static class IServiceCollectionExtensions
{
    extension(IServiceCollection services) 
    {
        public IServiceCollection AddAuthenticationSchemes()
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
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
                            var token = context.Token;
                            if (String.IsNullOrEmpty(token))
                                return;

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
