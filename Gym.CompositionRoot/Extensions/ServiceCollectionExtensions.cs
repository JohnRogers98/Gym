using Gym.Abstractions.MessageBus;
using Gym.Application;
using Gym.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gym.CompositionRoot.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddCompositionRoot(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg =>
            {
                cfg.LicenseKey = configuration["MEDIATR_LICENSE_KEY"];
                cfg.Lifetime = ServiceLifetime.Scoped;
                cfg.RegisterServicesFromAssembly(typeof(Application.DependencyInjection).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(Abstractions.MessageBus.DependencyInjection).Assembly);
            });

            services.AddInfrastructure(configuration);
            services.AddApplication(configuration);
            services.AddMessageBus(configuration);
            return services;
        }
    }
}
