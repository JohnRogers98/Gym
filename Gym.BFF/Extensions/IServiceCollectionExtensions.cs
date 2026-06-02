using Gym.BFF.Options;
using Gym.BFF.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Session;

namespace Gym.BFF.Extensions
{
    public static class IServiceCollectionExtensions
    {
        extension(IServiceCollection services) { 
        
            public IServiceCollection AddBffAuthentication()
            {
                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.Cookie.Name = "__Host-Gym.BFF>.Client";
                        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                        options.Cookie.HttpOnly = true;
                        options.Cookie.SameSite = SameSiteMode.Strict;
                        options.Cookie.Path = SessionDefaults.CookiePath;
                        options.Cookie.Domain = null;
                        options.Cookie.IsEssential = true;
                    });

                return services;
            }

            public IServiceCollection AddBffCors()
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("BffCorsPolicy", policy =>
                    {
                        policy.WithOrigins("https://localhost")
                              .WithHeaders("X-Static-Header")
                              .AllowCredentials();
                    });
                });

                return services;
            }

            public IServiceCollection AddAuthorizationServerNamedClient(String key)
            {
                services.AddHttpClient(key, httpClient =>
                {
                    httpClient.BaseAddress = new Uri("https://localhost:7218");
                });

                return services;
            }

            public IServiceCollection AddServerSideSession()
            {
                services.AddDistributedMemoryCache();
                services.AddSession(options =>
                {
                    options.Cookie.Name = "__Host-Gym.BFF.Server";
                    /*options.Cookie.Path = "/";*/
                    options.IdleTimeout = TimeSpan.FromMinutes(30);
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });

                return services;
            }

            public IServiceCollection AddOptionsFromCongiguration(IConfiguration configuration)
            {
                services.AddOptions<ClientCredentialsOptions>()
                    .Bind(configuration.GetRequiredSection(ClientCredentialsOptions.SectionName))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();

                services.AddOptions<UrlsOptions>()
                    .Bind(configuration.GetRequiredSection(UrlsOptions.SectionName))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();

                return services;
            }

            public IServiceCollection AddServices()
            {
                services.AddSingleton<IRandomBase64StringGenerator, RandomBase64StringGenerator>();
                services.AddSingleton<ICodeChallengePairGenerator, CodeChallengePairGenerator>();
                services.AddSingleton<IOAuthStateGenerator, OAuthStateGenerator>();
                services.AddSingleton<IOAuthNonceGenerator, OAuthNonceGenerator>();

                return services;
            }

        }
    }
}
