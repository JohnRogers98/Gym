using Gym.AuthorizationServer.Client.Options;
using Gym.AuthorizationServer.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Gym.AuthorizationServer.Client;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection SetupOAuthClientConfiguration(
            Action<ClientCredentialsOptions> clientConfigureOptions,
            Action<AuthorizationServerOptions> authorizationServerConfigureOptions)
        {
            services.AddOptions<ClientCredentialsOptions>()
                .Configure(clientConfigureOptions)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<AuthorizationServerOptions>()
                .Configure(authorizationServerConfigureOptions)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            AddOAuthClientServices(services);

            return services;
        }
        internal IServiceCollection AddOAuthClientServices()
        {
            services.AddSingleton<IExchangeCodeForTokenService, ExchangeCodeForTokenService>();
            services.AddSingleton<IJwkKeyProvider, JwkKeyProvider>();
            services.AddSingleton<IRefreshTokenService, RefreshTokenService>();
            services.AddSingleton<ITelegramAssertionService, TelegramAssertionService>();
            services.AddSingleton<IGetUserInfoService, GetUserInfoService>();

            return services;
        }

        public IServiceCollection SetupOAuthProtectedResourceConfiguration(Action<AuthorizationServerOptions> authorizationServerConfigureOptions)
        {
            services.AddOptions<AuthorizationServerOptions>()
                .Configure(authorizationServerConfigureOptions)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            AddOAuthProtectedResourceServices(services);

            return services;
        }
        internal IServiceCollection AddOAuthProtectedResourceServices()
        {
            services.AddSingleton<IJwkKeyProvider, JwkKeyProvider>();
            services.AddSingleton<IGetUserInfoService, GetUserInfoService>();

            return services;
        }
    }
}
