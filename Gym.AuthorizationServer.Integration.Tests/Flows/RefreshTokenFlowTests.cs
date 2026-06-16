using Gym.AuthorizationServer.Infrastructure.Entities.Scopes;
using Gym.OAuth.Extensions;
using MongoDB.Bson;
using System.Net.Http.Json;

namespace Gym.AuthorizationServer.Integration.Tests.Flows
{
    [Collection<TestServerCollection>]
    public class RefreshTokenFlowTests(TestServerFixture _fixture, ITestOutputHelper _outputHelper) : IntegrationTest(_fixture, _outputHelper)
    {
        [Fact]
        public async Task Pass_Through_Refresh_Token_Flow()
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
            await databaseShaper.WithUserConsentAsync(
                new()
                {
                    ClientId = DatabaseShaper.DefaultClientId,
                    UserId = DatabaseShaper.DefaultUserId,
                    ProtectedResourceId = DatabaseShaper.DefaultProtectedResourceId,
                    GrantedScopes = [scope_1.ToInfo(), scope_2.ToInfo()]
                });
            var tokens = await databaseShaper.WithDefaultTokenPairAsync();

            var httpClient = Fixture.CreateClient();
            #endregion

            #region Token
            TokenRequest tokenRequest = new()
            {
                ClientId = DatabaseShaper.DefaultClientId,
                ClientSecret = DatabaseShaper.DefaultClientSecret,
                RedirectUri = DatabaseShaper.DefaultClientRedirectUri,
                GrantType = "refresh_token",
                Resource = DatabaseShaper.DefaultProtectedResourceAudienceUri,
                RefreshToken = tokens.refreshToken
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
            await databaseShaper.WithUserConsentAsync(
                new()
                {
                    ClientId = DatabaseShaper.DefaultClientId,
                    UserId = DatabaseShaper.DefaultUserId,
                    ProtectedResourceId = DatabaseShaper.DefaultProtectedResourceId,
                    GrantedScopes = [openidScope.ToInfo()]
                });
            var tokens = await databaseShaper.WithDefaultTokenPairAsync();

            var httpClient = Fixture.CreateClient();
            #endregion

            #region Token
            TokenRequest tokenRequest = new()
            {
                ClientId = DatabaseShaper.DefaultClientId,
                ClientSecret = DatabaseShaper.DefaultClientSecret,
                RedirectUri = DatabaseShaper.DefaultClientRedirectUri,
                GrantType = "refresh_token",
                RefreshToken = tokens.refreshToken,
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
