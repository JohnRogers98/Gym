using Gym.Application.Services.DomainEventPublisher;
using Gym.Domain._Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Gym.Application.Tests")]

namespace Gym.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddMediatR(cfg =>
            {
                cfg.LicenseKey = configuration["MEDIATR_LICENSE_KEY"];
                cfg.Lifetime = ServiceLifetime.Scoped;
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });

            services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
            services.AddScoped<ITrainingBookingService, TrainingBookingService>();

            return services;
        }

    }
}
