using Gym.AuthorizationServer.Integration.Tests.Antiforgery;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ITestOutputHelper = Xunit.ITestOutputHelper;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Gym.AuthorizationServer.Integration.Tests
{
    public partial class TestServerFixture : WebApplicationFactory<Program>
    {
        public TestServerFixture() : base()
        {
            base.ClientOptions.AllowAutoRedirect = false;
            base.ClientOptions.BaseAddress = new Uri("https://localhost");
            base.ClientOptions.HandleCookies = true;

            //Force HTTP server startup
            using (base.CreateDefaultClient()) { }
        }

        public async Task<AntiforgeryTokens> GetAntiforgeryTokensAsync(Func<HttpClient>? httpClientFactory = null, CancellationToken cancellationToken = default)
        {
            using var httpClient = httpClientFactory?.Invoke() ?? base.CreateClient();
            using var response = await httpClient.GetAsync(AntiforgeryTokenController.GetTokensUri, cancellationToken).ConfigureAwait(false);

            String json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<AntiforgeryTokens>(json)!;
        }

        public async Task<AntiforgeryTokens> GetAntiforgeryTokensAsync(HttpClient httpClient, CancellationToken cancellationToken = default)
        {
            using var response = await httpClient.GetAsync(AntiforgeryTokenController.GetTokensUri, cancellationToken).ConfigureAwait(false);

            String json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<AntiforgeryTokens>(json)!;
        }

        public virtual void ClearOutputHelper()
            => Server.Services.GetRequiredService<ITestOutputHelperAccessor>().OutputHelper = null;

        public virtual void SetOutputHelper(ITestOutputHelper? value)
            => Server.Services.GetRequiredService<ITestOutputHelperAccessor>().OutputHelper = value;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAntiforgeryTokenResource()
                .ReplaceServicesWithFakes()   
                .ConfigureLogging((loggingBuilder) => loggingBuilder.ClearProviders().AddXUnit());
        }
    }
}