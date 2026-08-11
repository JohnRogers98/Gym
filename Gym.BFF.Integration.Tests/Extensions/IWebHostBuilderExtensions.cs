using Gym.BFF.Integration.Tests.Rsa;
using Gym.BFF.Options;
using Microsoft.AspNetCore.Hosting;
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