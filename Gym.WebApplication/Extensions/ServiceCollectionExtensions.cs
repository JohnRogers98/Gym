using Gym.WebApplication.Features.Admin.Shared.Services;
using Gym.WebApplication.Providers;
using Gym.WebApplication.ViewModels;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Polly;
using Polly.Fallback;
using Polly.Registry;

namespace Gym.WebApplication.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClient(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddScoped<CookieHandler>();
            services.AddScoped(sp =>
            {
                var cookieHandler = sp.GetRequiredService<CookieHandler>();

                if (cookieHandler.InnerHandler == null)
                {
                    cookieHandler.InnerHandler = new HttpClientHandler();
                }

                return new HttpClient(cookieHandler)
                {
                    BaseAddress = new Uri(configuration["WebApiBaseUrl"]!)
                };
            });

            return services;
        }

        public static IServiceCollection AddResiliencePipelines(this IServiceCollection services)
        {
            services.AddResiliencePipeline<String, InstructorViewModel?>(nameof(GetInstructorByIdService), builder =>
            {
                builder
                    .AddFallback(new FallbackStrategyOptions<InstructorViewModel?>
                    {
                        FallbackAction = args => Outcome.FromResultAsValueTask((InstructorViewModel?)null)
                    })
                    .AddTimeout(TimeSpan.FromSeconds(5))
                    .AddRetry(new()
                    {
                        MaxRetryAttempts = 3,
                        ShouldHandle = new PredicateBuilder<InstructorViewModel?>()
                            .Handle<HttpRequestException>()
                            .HandleResult(response => response is null)
                    });
            });

            services.AddScoped<IPipelineProvider, PipelineProvider>();

            return services;
        }
    }

    public class CookieHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
