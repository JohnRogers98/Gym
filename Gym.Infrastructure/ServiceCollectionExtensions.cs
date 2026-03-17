using Gym.Infrastructure.Entities.Projections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Gym.Infrastructure
{
    internal static  class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectionHanlders(this IServiceCollection services)
        {
            var factoryTypes = typeof(IProjectionHandler).Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract &&
                            !t.IsInterface &&
                            typeof(IProjectionHandler).IsAssignableFrom(t) &&
                            t != typeof(CompositeProjectionHandler))
                .ToList();

            foreach (var type in factoryTypes)
            {
                services.TryAddScoped(type);
            }

            services.TryAddScoped<IProjectionHandler>(sp =>
            {
                var handlers = new List<IProjectionHandler>();
                foreach (var type in factoryTypes)
                {
                    var handler = sp.GetRequiredService(type) as IProjectionHandler;
                    if (handler != null)
                    {
                        handlers.Add(handler);
                    }
                }

                return new CompositeProjectionHandler(handlers);
            });

            return services;
        }
    }

}
