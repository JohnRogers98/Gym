using Gym.Abstractions.MessageBus.Publishers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Gym.Abstractions.MessageBus
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddScoped<IDomainEventPublisher, DomainEventPublisher>();
            return services;
        }
    }
}
