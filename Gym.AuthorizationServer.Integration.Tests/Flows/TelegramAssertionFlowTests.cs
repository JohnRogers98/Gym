using Gym.OAuth.Extensions;
using MongoDB.Bson;
using System.Net.Http.Json;

namespace Gym.AuthorizationServer.Integration.Tests.Flows
{
    [Collection<TestServerCollection>]
    public class TelegramAssertionFlowTests(TestServerFixture _fixture, ITestOutputHelper _outputHelper) : IntegrationTest(_fixture, _outputHelper)
    {
        [Fact]
        public async Task Pass_Through_Telegram_Assertion_Flow()
        {
            #region Given
            DatabaseShaper databaseShaper = new DatabaseShaper(Fixture);
            await databaseShaper.WithDefaultClientAsync();
            await databaseShaper.WithDefaultUserAsync();
            await databaseShaper.WithDefaultUserRoleAsync();
            await databaseShaper.WithDefaultProtectedResourceAsync();
            var scope_1 = await databaseShaper.WithScopeAsync(
                new()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    RoleId = DatabaseShaper.DefaultRoleId,
                    ProtectedResourceId = DatabaseShaper.DefaultProtectedResourceId,
                    Name = "scope_1"
                });
            var scope_2 = await databaseShaper.WithScopeAsync(
                new()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    RoleId = DatabaseShaper.DefaultRoleId,
                    ProtectedResourceId = DatabaseShaper.DefaultProtectedResourceId,
                    Name = "scope_2"
                });

            var httpClient = Fixture.CreateClient();
            #endregion

            #region Token
            TokenRequest tokenRequest = new()
            {
                ClientId = DatabaseShaper.DefaultClientId,
                ClientSecret = DatabaseShaper.DefaultClientSecret,
                Scope = "scope_1 scope_2",
                RedirectUri = DatabaseShaper.DefaultClientRedirectUri,
                GrantType = "urn:telegram:grant-type:webapp",
                Assertion = DatabaseShaper.DefaultTelegramAssertion,
                Resource = DatabaseShaper.DefaultProtectedResourceAudienceUri
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
        public async Task Pass_Through_Telegram_Assertion_Flow_Using_Open_Id_Connect()
        {
            #region Given
            DatabaseShaper databaseShaper = new DatabaseShaper(Fixture);
            await databaseShaper.WithDefaultClientAsync();
            await databaseShaper.WithDefaultUserAsync();
            await databaseShaper.WithDefaultUserRoleAsync();
            await databaseShaper.WithDefaultProtectedResourceAsync();
            var openidScope = await databaseShaper.WithScopeAsync(
                new()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    RoleId = DatabaseShaper.DefaultRoleId,
                    ProtectedResourceId = DatabaseShaper.DefaultProtectedResourceId,
                    Name = "openid"
                });

            var httpClient = Fixture.CreateClient();
            #endregion

            #region Token
            TokenRequest tokenRequest = new()
            {
                ClientId = DatabaseShaper.DefaultClientId,
                ClientSecret = DatabaseShaper.DefaultClientSecret,
                Scope = "openid",
                RedirectUri = DatabaseShaper.DefaultClientRedirectUri,
                GrantType = "urn:telegram:grant-type:webapp",
                Assertion = DatabaseShaper.DefaultTelegramAssertion,
                Resource = DatabaseShaper.DefaultProtectedResourceAudienceUri
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
