using Gym.AuthorizationServer.Controllers.Api;
using Gym.AuthorizationServer.Queries;
using System.Net.Http.Json;

namespace Gym.AuthorizationServer.Integration.Tests.Flows
{
    [Collection<TestServerCollection>]
    public class AuthorizationCodeFlowTests(TestServerFixture _fixture, ITestOutputHelper _outputHelper) : IntegrationTest(_fixture, _outputHelper)
    {
        [Fact]
        public async Task Pass_Through_Authorization_Code_Flow()
        {
            await Fixture.CreateClientAsync();
            await Fixture.CreateUserAsync();
            await Fixture.CreateUserFormCredentialsAsync();

            var client = Fixture.CreateClient();

            #region Authorize
            AuthorizeQuery authorizeQuery = new()
            {
                ClientId = TestServerFixture.DefaultClientId,
                ResponseType = "code",
                RedirectUri = TestServerFixture.DefaultClientRedirectUri,
                Scope = "scope_2",
                State = TestServerFixture.DefaultState
            };

            var authorizeResponse = await client.GetAsync($"/authorize{authorizeQuery.ToQueryString()}", TestContext.Current.CancellationToken);
            authorizeResponse.EnsureRedirectStatusCode();
            #endregion

            #region Login
            var redirectLoginResponse = await client.GetAsync(authorizeResponse.Headers.Location, TestContext.Current.CancellationToken);
            redirectLoginResponse.EnsureSuccessStatusCode();

            var tokens = await Fixture.GetAntiforgeryTokensAsync(client, TestContext.Current.CancellationToken);

            var loginFormData = new Dictionary<String, String>
            {
                [tokens.FormFieldName] = tokens.RequestToken,
                ["Username"] = TestServerFixture.DefaultUserUsername,
                ["Password"] = TestServerFixture.DefaultUserPassword
            };
            var loginFormContent = new FormUrlEncodedContent(loginFormData);

            var loginPostResponse = await client.PostAsync("/login", loginFormContent, TestContext.Current.CancellationToken);
            loginPostResponse.EnsureRedirectStatusCode();
            #endregion

            #region Approve
            var redirectApproveResponse = await client.GetAsync(loginPostResponse.Headers.Location, TestContext.Current.CancellationToken);
            redirectApproveResponse.EnsureSuccessStatusCode();

            tokens = await Fixture.GetAntiforgeryTokensAsync(client, TestContext.Current.CancellationToken);

            var approveFormData = new Dictionary<String, String>
            {
                [tokens.FormFieldName] = tokens.RequestToken,
                ["Scopes[0].IsSelected"] = "true",
                ["Scopes[0].Name"] = "scope_2"
            };
            var approveFormContent = new FormUrlEncodedContent(approveFormData);

            var approvePostResponse = await client.PostAsync("/approve", approveFormContent, TestContext.Current.CancellationToken);
            approvePostResponse.EnsureRedirectStatusCode();

            var queryString = approvePostResponse.Headers.Location!.ToString()
                .Substring(approvePostResponse.Headers.Location!.ToString().IndexOf('?'));

            var queryParams = System.Web.HttpUtility.ParseQueryString(queryString);
               
            var code = queryParams["code"];
            var state = queryParams["state"];

            Assert.NotNull(code);
            Assert.Equal(TestServerFixture.DefaultState, state);
            #endregion

            #region Token
            TokenRequest tokenRequest = new()
            {
                ClientId = TestServerFixture.DefaultClientId,
                ClientSecret = TestServerFixture.DefaultClientSecret,
                RedirectUri = TestServerFixture.DefaultClientRedirectUri,
                GrantType = "authorization_code",
                Code = code
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
