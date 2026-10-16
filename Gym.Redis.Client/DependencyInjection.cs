using Gym.Redis.Client;
using Gym.Redis.Client.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.StackExchangeRedis;
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

        public IServiceCollection AddIdempotencyServices()
        {
            services.AddSingleton<ISaveIdempotencyRecordService, SaveIdempotencyRecordService>();
            services.AddSingleton<IGetIdempotencyRecordService, GetIdempotencyRecordService>();

            return services;
        }

        public IServiceCollection AddSessionServices()
        {
            services.AddSingleton<ISaveSessionTokensService, SaveSessionTokensService>();
            services.AddSingleton<IGetSessionTokensService, GetSessionTokensService>();
            services.AddSingleton<IRefreshSessionTokensService, RefreshSessionTokensService>();

            return services;
        }

        public IServiceCollection AddRedisDataProtection(String applicationName)
        {
            services.AddDataProtection()
                .SetApplicationName(applicationName);

            // Register RedisXmlRepository through IConfigureOptions as a workaround for the fact that DataProtection is registered as a singleton and we need to resolve IConnectionMultiplexer from the DI container. See https://github.com/dotnet/aspnetcore/issues/61768
            services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(sp =>
            {
                var redis = sp.GetRequiredService<IConnectionMultiplexer>();
                return new ConfigureOptions<KeyManagementOptions>(options =>
                {
                    options.XmlRepository = new RedisXmlRepository(
                        () => redis.GetDatabase(),
                        "DataProtection-Keys");
                });
            });

            return services;
        }
    }
}