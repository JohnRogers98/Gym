using Gym.AuthorizationServer.Controllers.Api;
using Gym.AuthorizationServer.Entities.UserConsents;
using System.Net.Http.Json;

namespace Gym.AuthorizationServer.Integration.Tests.Flows
{
    [Collection<TestServerCollection>]
    public class RefreshTokenFlowTests(TestServerFixture _fixture, ITestOutputHelper _outputHelper) : IntegrationTest(_fixture, _outputHelper)
    {
        [Fact]
        public async Task Pass_Through_Refresh_Token_Flow()
        {
            await Fixture.CreateOAuthClientAsync();
            await Fixture.CreateOAuthUserAsync();
            await Fixture.CreateOAuthUserConsentAsync();
            var tokenPair = await Fixture.CreateOAuthTokenPairAsync();

            var httpClient = Fixture.CreateClient();

            #region Token
            TokenRequest tokenRequest = new()
            {
                ClientId = TestServerFixture.DefaultClientId,
                ClientSecret = TestServerFixture.DefaultClientSecret,
                RedirectUri = TestServerFixture.DefaultClientRedirectUri,
                GrantType = "refresh_token",
                RefreshToken = tokenPair.refreshToken
            };

            var tokenPostResponse = await httpClient.PostAsync("/token", tokenRequest.ToFormContent(), TestContext.Current.CancellationToken);
            tokenPostResponse.EnsureSuccessStatusCode();

            var tokenResponse = await tokenPostResponse.Content.ReadFromJsonAsync<TokenResponse>(TestContext.Current.CancellationToken);

            Assert.NotNull(tokenResponse);
            Assert.NotNull(tokenResponse.AccessToken);
            Assert.NotNull(tokenResponse.RefreshToken);
            Assert.Null(tokenResponse.IdToken);
            #endregion
        }

        [Fact]
        public async Task Pass_Through_Refresh_Token_Flow_Using_Open_Id_Connect()
        {
            await Fixture.CreateOAuthClientAsync(new()
            {
                Id = TestServerFixture.DefaultClientId,
                Name = "test_client",
                RedirectUri = TestServerFixture.DefaultClientRedirectUri,
                Scope = ["openid"],
                SecretHash = TestServerFixture.DefaultClientSecretHash
            });

            await Fixture.CreateOAuthUserAsync();

            UserConsentEntity consent = new()
            {
                ClientId = TestServerFixture.DefaultClientId,
                UserId = TestServerFixture.DefaultUserId,
                GrantedScopes = ["openid"]
            };
            await Fixture.CreateOAuthUserConsentAsync(consent);
            var tokenPair = await Fixture.CreateOAuthTokenPairAsync();

            var httpClient = Fixture.CreateClient();

            #region Token
            TokenRequest tokenRequest = new()
            {
                ClientId = TestServerFixture.DefaultClientId,
                ClientSecret = TestServerFixture.DefaultClientSecret,
                RedirectUri = TestServerFixture.DefaultClientRedirectUri,
                GrantType = "refresh_token",
                RefreshToken = tokenPair.refreshToken
            };

            var tokenPostResponse = await httpClient.PostAsync("/token", tokenRequest.ToFormContent(), TestContext.Current.CancellationToken);
            tokenPostResponse.EnsureSuccessStatusCode();

            var tokenResponse = await tokenPostResponse.Content.ReadFromJsonAsync<TokenResponse>(TestContext.Current.CancellationToken);

            Assert.NotNull(tokenResponse);
            Assert.NotNull(tokenResponse.AccessToken);
            Assert.NotNull(tokenResponse.RefreshToken);
            Assert.NotNull(tokenResponse.IdToken);
            #endregion
        }
    }
}
