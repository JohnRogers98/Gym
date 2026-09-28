using Gym.Redis.Client;
using Gym.Redis.Client.Services;
using Microsoft.Extensions.Options;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddRedis(Action<RedisOptions> configureOptions)
        {
            services.AddOptions<RedisOptions>()
                .Configure(configureOptions)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.ConfigureConnection();
            services.AddServices();

            return services;
        }

        private IServiceCollection ConfigureConnection()
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
                return ConnectionMultiplexer.Connect(options.ConnectionString);
            });

            services.AddSingleton<RedLockFactory>(sp =>
            {
                var connectionMultiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
                RedLockMultiplexer redLockMuxer = (ConnectionMultiplexer)connectionMultiplexer; //implicit conversion
                
                var redLockMultiplexers = new List<RedLockMultiplexer>
                {
                    redLockMuxer
                };
                return RedLockFactory.Create(redLockMultiplexers);
            });

            return services;
        }

        private IServiceCollection AddServices()
        {
            services.AddSingleton<ISaveSessionTokensService, SaveSessionTokensService>();
            services.AddSingleton<IGetSessionTokensService, GetSessionTokensService>();
            services.AddSingleton<IRefreshSessionTokensService, RefreshSessionTokensService>();

            return services;
        }
    }
}