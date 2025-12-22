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
            services.AddInfrastructure(configuration);
            services.AddApplication(configuration);
            return services;
        }
    }
}
