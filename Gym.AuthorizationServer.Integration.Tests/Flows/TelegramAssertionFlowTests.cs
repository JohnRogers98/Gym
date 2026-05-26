using Gym.AuthorizationServer.Controllers.Api;
using System.Net.Http.Json;

namespace Gym.AuthorizationServer.Integration.Tests.Flows
{
    [Collection<TestServerCollection>]
    public class TelegramAssertionFlowTests(TestServerFixture _fixture, ITestOutputHelper _outputHelper) : IntegrationTest(_fixture, _outputHelper)
    {
        [Fact]
        public async Task Pass_Through_Telegram_Assertion_Flow()
        {
            await Fixture.CreateClientAsync();

            var client = Fixture.CreateClient();

            #region Token
            TokenRequest tokenRequest = new()
            {
                ClientId = TestServerFixture.DefaultClientId,
                ClientSecret = TestServerFixture.DefaultClientSecret,
                Scope = "scope_1 scope_2",
                RedirectUri = TestServerFixture.DefaultClientRedirectUri,
                GrantType = "urn:telegram:grant-type:webapp",
                Assertion = TestServerFixture.DefaultTelegramAssertion
            };

            var tokenPostResponse = await client.PostAsync("/token", tokenRequest.ToFormContent(), TestContext.Current.CancellationToken);
            tokenPostResponse.EnsureSuccessStatusCode();

            var tokenResponse = await tokenPostResponse.Content.ReadFromJsonAsync<TokenResponse>(TestContext.Current.CancellationToken);

            Assert.NotNull(tokenResponse);
            Assert.NotNull(tokenResponse.AccessToken);
            Assert.NotNull(tokenResponse.RefreshToken);
            #endregion
        }
    }
}
