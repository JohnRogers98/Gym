using Gym.BFF.DelegatingHandlers;
using Gym.BFF.Integration.Tests.Controllers;
using Gym.BFF.Integration.Tests.Options;
using Gym.BFF.Integration.Tests.Rsa;
using Gym.BFF.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace Gym.BFF.Integration.Tests.Extensions;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class IWebHostBuilderExtensions
{
    public static IWebHostBuilder AddApplicationParts(this IWebHostBuilder builder)
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        return builder.ConfigureServices((services) =>
        {
            services.AddControllers()
                    .AddApplicationPart(typeof(BFFServerFixture).Assembly);
        });
    }

    public static IWebHostBuilder AddFakeRsaInfrastructure(this IWebHostBuilder builder)
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        return builder.ConfigureServices((services) =>
        {
            services.AddSingleton<FakeRsaKeyProvider>();
            services.AddSingleton<FakeRsaSecutiryKey>();
        });
    }

    public static IWebHostBuilder ReconfigureXStaticHeaderExcludedEndpoints(this IWebHostBuilder builder)
    {
        return builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<String, String?>
            {
                // Переопределяем весь массив целиком
                { "StaticHeaderCheck:ExcludedPaths:0", "login" },
                { "StaticHeaderCheck:ExcludedPaths:1", "callback" },
                { "StaticHeaderCheck:ExcludedPaths:2", "logout" },
                { "StaticHeaderCheck:ExcludedPaths:3", FakeProtectedResourceProxyController.GetUri.OriginalString }
            });
        });
    }

    public static IWebHostBuilder AddProtectedResourceOptions(this IWebHostBuilder builder,
        String protectedResourceBaseUrl, String clientName = "protected-resource-client")
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        builder.UseSetting("Urls:TestProtectedResource:ClientName", clientName);
        builder.UseSetting("Urls:TestProtectedResource:BaseUrl", protectedResourceBaseUrl);

        return builder.ConfigureServices((context, services) =>
        {
            services.AddOptions<ProtectedResourceOptions>()
                .Bind(context.Configuration.GetSection("Urls:TestProtectedResource"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddHttpClient(clientName, client =>
            {
                client.BaseAddress = new Uri(protectedResourceBaseUrl);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                UseCookies = false,
                UseDefaultCredentials = false
            })
            .AddHttpMessageHandler<AccessTokenHandler>();
        });
    }

    /// <summary>
    /// Need to exclude because of WireMock is running in the same process, so middleware catch requests.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IWebHostBuilder ExludeAuthorizeServerEndpoints(this IWebHostBuilder builder)
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        return builder.ConfigureServices(services =>
        {
            services.PostConfigure<StaticHeaderCheckOptions>(options =>
            {
                options.ExcludedPaths.Add("/authorize");
                options.ExcludedPaths.Add("/token");
                options.ExcludedPaths.Add("/.well-known/jwks.json");
                options.ExcludedPaths.Add("/userinfo");
            });
        });
    }
}