using Gym.AuthorizationServer.Infrastructure.Entities.Scopes;
using MongoDB.Bson;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Gym.AuthorizationServer.Integration.Tests.UserInfo
{
    [Collection<TestServerCollection>]
    public class UserInfoTests(TestServerFixture _fixture, ITestOutputHelper _outputHelper) : IntegrationTest(_fixture, _outputHelper)
    {
        [Fact]
        public async Task User_Info_Accept_Token_And_Generate_Response()
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
            var profileScope = await databaseShaper.WithScopeAsync(
                new()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    RoleId = DatabaseShaper.DefaultRoleId,
                    ProtectedResourceId = DatabaseShaper.DefaultProtectedResourceId,
                    Name = "profile"
                });
            await databaseShaper.WithUserConsentAsync(
                new()
                {
                    ClientId = DatabaseShaper.DefaultClientId,
                    UserId = DatabaseShaper.DefaultUserId,
                    ProtectedResourceId = DatabaseShaper.DefaultProtectedResourceId,
                    GrantedScopes = [openidScope.ToInfo(), profileScope.ToInfo()]
                });
            await databaseShaper.WithDefaultUserFormCredentialsAsync();
            var tokens = await databaseShaper.WithDefaultTokenPairAsync();

            var httpClient = Fixture.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "userinfo");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);
            #endregion

            //When
            var userInfoResponse = await httpClient.SendAsync(request, TestContext.Current.CancellationToken);
            var userInfoObject = await userInfoResponse.Content.ReadFromJsonAsync<OAuth.Extensions.UserInfo>(TestContext.Current.CancellationToken);

            //Then
            Assert.NotNull(userInfoObject);
            Assert.Equal(DatabaseShaper.DefaultUserId, userInfoObject.Subject);
            Assert.Equal("John Doe", userInfoObject.Name);
            Assert.Equal("John", userInfoObject.GivenName);
            Assert.Equal("Doe", userInfoObject.FamilyName);
        }
    }
}
