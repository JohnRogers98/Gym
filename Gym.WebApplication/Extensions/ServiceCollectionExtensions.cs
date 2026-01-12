using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Gym.WebApplication.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClient(this IServiceCollection services) 
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
                    BaseAddress = new Uri("https://localhost:7246")
                };
            });

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
