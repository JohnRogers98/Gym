using Gym.BFF.Integration.Tests.Extensions;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WireMock.Logging;
using WireMock.Server;
using WireMock.Settings;
using ITestOutputHelper = Xunit.ITestOutputHelper;

[assembly: CaptureConsole]

namespace Gym.BFF.Integration.Tests;

public partial class BFFServerFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    public WireMockServer AuthorizationServerMock { get; private set; }

    public BFFServerFixture() : base()
    {
        WireMockServerSettings authorizationServerSettings = new()
        {
            Logger = new WireMockConsoleLogger()
        };
        AuthorizationServerMock = WireMockServer.Start(authorizationServerSettings);

        base.ClientOptions.AllowAutoRedirect = false;
        base.ClientOptions.BaseAddress = new Uri("https://localhost");
        base.ClientOptions.HandleCookies = true;

        //Force HTTP server startup
        using var client = CreateDefaultClient();
    }

    public virtual void ClearOutputHelper()
        => Server.Services.GetRequiredService<ITestOutputHelperAccessor>().OutputHelper = null;

    public virtual void SetOutputHelper(ITestOutputHelper? value)
        => Server.Services.GetRequiredService<ITestOutputHelperAccessor>().OutputHelper = value;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .AddApplicationParts()
            .AddFakeRsaInfrastructure()
            .ExludeAuthorizeServerEndpoints()
            .UseSetting("Urls:AuthorizationServer:BaseUrl", AuthorizationServerMock.Url)
            .ConfigureLogging((loggingBuilder) => loggingBuilder.ClearProviders().AddXUnit());
    }

    public async ValueTask InitializeAsync() { }

    private Boolean _disposed;
    public async override ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            AuthorizationServerMock.Stop();
            AuthorizationServerMock.Dispose();
            _disposed = true;
        }

        await base.DisposeAsync();
    }
}