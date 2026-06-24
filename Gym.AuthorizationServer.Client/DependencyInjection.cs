using Gym.AuthorizationServer.Client.Options;
using Gym.AuthorizationServer.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Gym.AuthorizationServer.Client;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection SetupOAuthClientConfiguration(ClientCredentialsOptions clientCredentialsOptions, AuthorizationServerOptions authorizationServerOptions)
        {
            services.TryAddSingleton(_ => clientCredentialsOptions);
            services.TryAddSingleton(_ => authorizationServerOptions);

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

        public IServiceCollection SetupOAuthProtectedResourceConfiguration(AuthorizationServerOptions authorizationServerOptions)
        {
            services.TryAddSingleton(_ => authorizationServerOptions);

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
