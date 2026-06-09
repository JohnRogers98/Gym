using Gym.BFF.Options;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WireMock.Server;

namespace Gym.BFF.Integration.Tests.Tests
{
    [Collection<BFFServerCollection>]
    public class LogoutTests(BFFServerFixture _fixture, ITestOutputHelper _outputHelper) : IntegrationTest(_fixture, _outputHelper)
    {
        [Fact]
        public async Task Check_Logout()
        {
            #region Given
            var httpClient = Fixture.CreateClient();

            var urls = Fixture.Services.GetRequiredService<IOptions<UrlsOptions>>().Value;

            Fixture.AuthorizationServerMock.SetupExchageCodeToken(urls.AuthorizationServer.TokenEndpoint, "test_access_token", "test_refresh_token");

            var loginResponse = await httpClient.GetAsync("/login", TestContext.Current.CancellationToken);
            var queryParams = QueryHelpers.ParseQuery(loginResponse.Headers.Location!.Query);
            String state = queryParams["state"]!;

            var callbackResponse = await httpClient.GetAsync($"/callback?code=test_code&state={state}", TestContext.Current.CancellationToken);
            callbackResponse.EnsureSuccessStatusCode();
            #endregion

            var request = new HttpRequestMessage(HttpMethod.Get, "api/check-session");
            request.Headers.AddXStaticHeader();

            var checkSessionBeforeLogout = await httpClient.SendAsync(request, TestContext.Current.CancellationToken);
            var checkSessionBeforeLogoutResponse = await checkSessionBeforeLogout.Content.ReadFromJsonAsync<SessionResponse>(TestContext.Current.CancellationToken);
            Assert.True(checkSessionBeforeLogoutResponse!.Authenticated);

            var logoutResponse = await httpClient.PostAsync("/logout", null, cancellationToken: TestContext.Current.CancellationToken);
            callbackResponse.EnsureSuccessStatusCode();

            request = new HttpRequestMessage(HttpMethod.Get, "api/check-session");
            request.Headers.AddXStaticHeader();

            var checkSessionAfterLogout = await httpClient.SendAsync(request, TestContext.Current.CancellationToken);
            var checkSessionAfterLogoutResponse = await checkSessionAfterLogout.Content.ReadFromJsonAsync<SessionResponse>(TestContext.Current.CancellationToken);
            Assert.False(checkSessionAfterLogoutResponse!.Authenticated);
        }
    }

    public class SessionResponse
    {
        public Boolean Authenticated { get; set; }
    }
}
