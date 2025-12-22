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
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });

            return services;
        }

    }
}
