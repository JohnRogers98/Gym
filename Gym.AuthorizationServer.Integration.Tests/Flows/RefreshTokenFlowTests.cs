using Gym.AuthorizationServer.Controllers.Api;
using System.Net.Http.Json;

namespace Gym.AuthorizationServer.Integration.Tests.Flows
{
    [Collection<TestServerCollection>]
    public class RefreshTokenFlowTests(TestServerFixture _fixture, ITestOutputHelper _outputHelper) : IntegrationTest(_fixture, _outputHelper)
    {
        [Fact]
        public async Task Pass_Through_Refresh_Token_Flow()
        {
            await Fixture.CreateClientAsync();
            await Fixture.CreateUserAsync();
            await Fixture.CreateUserConsentAsync();
            var tokenPair = await Fixture.CreateTokenPairAsync();

            var client = Fixture.CreateClient();

            #region Token
            TokenRequest tokenRequest = new()
            {
                ClientId = TestServerFixture.DefaultClientId,
                ClientSecret = TestServerFixture.DefaultClientSecret,
                RedirectUri = TestServerFixture.DefaultClientRedirectUri,
                GrantType = "refresh_token",
                RefreshToken = tokenPair.refreshToken
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
