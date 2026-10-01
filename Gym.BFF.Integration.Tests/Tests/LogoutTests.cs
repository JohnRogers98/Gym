using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Gym.BFF.Integration.Tests.Tests
{
    [Collection<BFFServerCollection>]
    public class LogoutTests(BFFServerFixture _fixture, ITestOutputHelper _outputHelper) 
        : ExtendedIntegrationTest(_fixture, _outputHelper)
    {
        [Fact]
        public async Task Check_Logout()
        {
            #region Given
            using var httpClient = Fixture.CreateClient();

            String refreshToken = Guid.NewGuid().ToString();
            String accessToken = base.CreateJwtToken();
            await base.MakeLoginCallToSetupSesionCookieAsync(httpClient, accessToken, refreshToken);
            #endregion

            using var request = new HttpRequestMessage(HttpMethod.Get, "/check-session");
            request.Headers.AddXStaticHeader();

            var checkSessionBeforeLogout = await httpClient.SendAsync(request, TestContext.Current.CancellationToken);
            var checkSessionBeforeLogoutResponse = await checkSessionBeforeLogout.Content.ReadFromJsonAsync<SessionResponse>(TestContext.Current.CancellationToken);
            Assert.True(checkSessionBeforeLogoutResponse!.Authenticated);

            var logoutResponse = await httpClient.PostAsync("/logout", null, cancellationToken: TestContext.Current.CancellationToken);
            logoutResponse.EnsureSuccessStatusCode();

            using var newRequest = new HttpRequestMessage(HttpMethod.Get, "/check-session");
            newRequest.Headers.AddXStaticHeader();

            var checkSessionAfterLogout = await httpClient.SendAsync(newRequest, TestContext.Current.CancellationToken);
            var checkSessionAfterLogoutResponse = await checkSessionAfterLogout.Content.ReadFromJsonAsync<SessionResponse>(TestContext.Current.CancellationToken);
            Assert.False(checkSessionAfterLogoutResponse!.Authenticated);
        }
    }

    public class SessionResponse
    {
        public Boolean Authenticated { get; set; }
    }
}
