using Gym.BFF.DelegatingHandlers;
using Gym.BFF.Options;
using Gym.BFF.Services;
using Gym.BFF.Services.Session;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Session;

namespace Gym.BFF.Extensions;

public static class IServiceCollectionExtensions
{
    extension(IServiceCollection services) { 
    
        public IServiceCollection AddBffAuthentication()
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "__Host-Gym.BFF.Client";
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.Cookie.Path = SessionDefaults.CookiePath;
                    options.Cookie.Domain = null;
                    options.Cookie.IsEssential = true;
                });

            return services;
        }

        public IServiceCollection AddServerSideSession()
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = "__Host-Gym.BFF.Server";
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            return services;
        }

        public IServiceCollection AddCorsPolicy(String policyName, String spaUrl)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(policyName, policy =>
                {
                    policy.WithOrigins(spaUrl)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            return services;
        }
        public IServiceCollection AddDelegatingHandlers()
        {
            services.AddTransient<AddForwardHeadersHandler>();
            services.AddTransient<RefreshTokenHandler>();
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
            })
            .AddHttpMessageHandler<AddForwardHeadersHandler>()
            .AddHttpMessageHandler<RefreshTokenHandler>();

            return services;
        }

        public IServiceCollection AddAuthorizationServerAdminApiNamedClient(String key, String baseUrl)
        {
            services.AddHttpClient(key, client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                UseCookies = false,
                UseDefaultCredentials = false
            })
            .AddHttpMessageHandler<AddForwardHeadersHandler>()
            .AddHttpMessageHandler<RefreshTokenHandler>();

            return services;
        }

        public IServiceCollection AddOptionsFromConfiguration(IConfiguration configuration)
        {
            services.AddOptions<StaticHeaderCheckOptions>()
                .Bind(configuration.GetRequiredSection(StaticHeaderCheckOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<ResourceUrisOptions>()
                .Bind(configuration.GetRequiredSection(ResourceUrisOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<AuthorizationServerAdminApiOptions>()
                .Bind(configuration.GetRequiredSection(AuthorizationServerAdminApiOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<SpaOptions>()
                .Bind(configuration.GetRequiredSection(SpaOptions.SectionName))
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
            services.AddSingleton<IComputeOpenIdAtHashService, ComputeOpenIdAtHashService>();
            services.AddSingleton<IRsaSecurityKeyProvider, RsaSecurityKeyProvider>();
            services.AddSingleton<IOAuthIdTokenValidator, OAuthIdTokenValidator>();
            services.AddSingleton<ISetTokensToClientSideSessionService, SetTokensToClientSideSessionService>();

            return services;
        }

    }
}
