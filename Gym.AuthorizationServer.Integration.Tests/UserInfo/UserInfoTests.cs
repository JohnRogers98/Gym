using Gym.AuthorizationServer.Controllers.Api;
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
            //Given
            await Fixture.CreateOAuthClientAsync(new()
            {
                Id = TestServerFixture.DefaultClientId,
                Name = "test_client",
                RedirectUri = TestServerFixture.DefaultClientRedirectUri,
                Scope = ["openid", "profile"],
                SecretHash = TestServerFixture.DefaultClientSecretHash
            });

            await Fixture.CreateOAuthUserAsync(new()
            {
                Id = TestServerFixture.DefaultUserId,
                FirstName = "John", 
                LastName = "Doe",
                Role = "client"
            });

            await Fixture.CreateOAuthUserConsentAsync(new()
            {
                ClientId = TestServerFixture.DefaultClientId,
                UserId = TestServerFixture.DefaultUserId,
                GrantedScopes = ["openid", "profile"]
            });

            await Fixture.CreateOAuthUserFormCredentialsAsync();
            var tokens = await Fixture.CreateOAuthTokenPairAsync();

            var httpClient = Fixture.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "userinfo");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            //When
            var userInfoResponse = await httpClient.SendAsync(request, TestContext.Current.CancellationToken);
            var userInfoObject = await userInfoResponse.Content.ReadFromJsonAsync<UserInfoResponse>(TestContext.Current.CancellationToken);

            //Then
            Assert.NotNull(userInfoObject);
            Assert.Equal(TestServerFixture.DefaultUserId, userInfoObject.Subject);
            Assert.Equal("John Doe", userInfoObject.Name);
            Assert.Equal("John", userInfoObject.GivenName);
            Assert.Equal("Doe", userInfoObject.FamilyName);
        }
    }
}
