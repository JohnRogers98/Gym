using Gym.RabbitMQ.Topology;
using Gym.RabbitMQ.Topology.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    extension(IServiceCollection services) 
    { 
        public IServiceCollection AddRabbitMQConnection(Action<RabbitMQOptions> configureOptions)
        {
            services.AddOptions<RabbitMQOptions>()
                .Configure(configureOptions)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.ConfigureConnection();
            services.AddTopologyInitializer();
            services.AddPublishSubscribeServices();

            return services;
        }

        private IServiceCollection ConfigureConnection()
        {
            services.AddSingleton<IConnection>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<RabbitMQOptions>>().Value;

                var factory = new ConnectionFactory
                {
                    HostName = options.Hostname,
                    UserName = options.Username,
                    Password = options.Password,
                    VirtualHost = options.Vhost,
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
                };

                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            });

            return services;
        }

        private IServiceCollection AddTopologyInitializer()
        {
            services.AddSingleton<IRabbitMQTopologyInitializer, RabbitMQTopologyInitializer>();
            return services;
        }

        private IServiceCollection AddPublishSubscribeServices()
        {
            services.AddSingleton<IUserCreatedEventService, UserCreatedEventService>();

            return services;
        }

    }
}
