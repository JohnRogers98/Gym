using Gym.AuthorizationServer.Integration.Tests;
using Gym.AuthorizationServer.Integration.Tests.Antiforgery;
using Gym.AuthorizationServer.Integration.Tests.Fakes;
using Gym.AuthorizationServer.Services.Rsa;
using Microsoft.AspNetCore.Hosting;
using MongoDB.Driver;
using System.ComponentModel;

namespace Microsoft.Extensions.DependencyInjection
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IWebHostBuilderExtensions
    {
        public static IWebHostBuilder ConfigureAntiforgeryTokenResource(this IWebHostBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            return builder.ConfigureServices((services) =>
            {
                services.AddControllers()
                        .AddApplicationPart(typeof(AntiforgeryTokenController).Assembly);
            });
        }

        public static IWebHostBuilder ReplaceServicesWithFakes(this IWebHostBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.ConfigureServices(services =>
            {
                services
                    .ReplaceService<IMongoDatabase>(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(TestServerFixture.DefaultTestDatabase))
                    .ReplaceService<IRsaKeyService, FakeRsaKeyService>();
            }); 

            builder.UseEnvironment("Development");

            return builder;
        }

        private static IServiceCollection ReplaceService<TService>(this IServiceCollection services, Func<IServiceProvider, TService> factory, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) 
            where TService : class
        {
            var serviceDescriptor = services
                .SingleOrDefault(aService => aService.ServiceType == typeof(TService));

            if (serviceDescriptor is not null)
            {
                var originalLifetime = serviceDescriptor.Lifetime;
                services.Remove(serviceDescriptor);
                services.Add(new ServiceDescriptor(typeof(TService), factory, originalLifetime));
            }
            else
            {
                services.Add(new ServiceDescriptor(typeof(TService), factory, serviceLifetime));
            }

            return services;
        }

        private static IServiceCollection ReplaceService<TService, TReplacing>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TService : class where TReplacing : class, TService
        {
            var serviceDescriptor = services
                .SingleOrDefault(aService => aService.ServiceType == typeof(TService));

            if (serviceDescriptor is not null)
            {
                var originalLifetime = serviceDescriptor.Lifetime;
                services.Remove(serviceDescriptor);
                services.Add(new ServiceDescriptor(typeof(TService), typeof(TReplacing), originalLifetime));
            }
            else
            {
                services.Add(new ServiceDescriptor(typeof(TService), typeof(TReplacing), serviceLifetime));
            }

            return services;
        }
    }
}