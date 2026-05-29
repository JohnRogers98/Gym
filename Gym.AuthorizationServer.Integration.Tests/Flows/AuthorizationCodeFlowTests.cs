using Gym.AuthorizationServer.Controllers.Api;
using Gym.AuthorizationServer.Queries;
using Gym.AuthorizationServer.Services.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Json;
using System.Security.Cryptography;

namespace Gym.AuthorizationServer.Integration.Tests.Flows
{
    [Collection<TestServerCollection>]
    public class AuthorizationCodeFlowTests(TestServerFixture _fixture, ITestOutputHelper _outputHelper) : IntegrationTest(_fixture, _outputHelper)
    {
        [Fact]
        public async Task Pass_Through_Authorization_Code_Flow()
        {
            await Fixture.CreateOAuthClientAsync();
            await Fixture.CreateOAuthUserAsync();
            await Fixture.CreateOAuthUserFormCredentialsAsync();

            var authorizeQuery = new AuthorizeQuery
            {
                ClientId = TestServerFixture.DefaultClientId,
                ResponseType = "code",
                RedirectUri = TestServerFixture.DefaultClientRedirectUri,
                Scope = "scope_2",
                State = TestServerFixture.DefaultState,
                CodeChallenge = TestServerFixture.DefaultCodeChallange,
                CodeChallengeMethod = TestServerFixture.DefaultCodeChallangeMethod
            };

            await RunAuthorizationCodeFlow(authorizeQuery, expectIdToken: false);
        }

        [Fact]
        public async Task Pass_Through_Authorization_Code_Flow_Using_Open_Id_Connect()
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
            await Fixture.CreateOAuthUserFormCredentialsAsync();

            var authorizeQuery = new AuthorizeQuery
            {
                ClientId = TestServerFixture.DefaultClientId,
                ResponseType = "code",
                RedirectUri = TestServerFixture.DefaultClientRedirectUri,
                Scope = "openid",
                State = TestServerFixture.DefaultState,
                CodeChallenge = TestServerFixture.DefaultCodeChallange,
                CodeChallengeMethod = TestServerFixture.DefaultCodeChallangeMethod,
                Nonce = TestServerFixture.DefaultNonce
            };

            await RunAuthorizationCodeFlow(authorizeQuery, expectIdToken: true);
        }

        private async Task RunAuthorizationCodeFlow(AuthorizeQuery authorizeQuery, Boolean expectIdToken)
        {
            var httpClient = Fixture.CreateClient();

            #region Authorize
            var authorizeResponse = await httpClient.GetAsync($"/authorize{authorizeQuery.ToQueryString()}", TestContext.Current.CancellationToken);
            authorizeResponse.EnsureRedirectStatusCode();
            #endregion

            #region Login
            var redirectLoginResponse = await httpClient.GetAsync(authorizeResponse.Headers.Location, TestContext.Current.CancellationToken);
            redirectLoginResponse.EnsureSuccessStatusCode();

            var tokens = await Fixture.GetAntiforgeryTokensAsync(httpClient, TestContext.Current.CancellationToken);

            var loginFormData = new Dictionary<String, String>
            {
                [tokens.FormFieldName] = tokens.RequestToken,
                ["Username"] = TestServerFixture.DefaultUserUsername,
                ["Password"] = TestServerFixture.DefaultUserPassword
            };
            var loginFormContent = new FormUrlEncodedContent(loginFormData);

            var loginPostResponse = await httpClient.PostAsync("/login", loginFormContent, TestContext.Current.CancellationToken);
            loginPostResponse.EnsureRedirectStatusCode();
            #endregion

            #region Approve
            var redirectApproveResponse = await httpClient.GetAsync(loginPostResponse.Headers.Location,TestContext.Current.CancellationToken);
            redirectApproveResponse.EnsureSuccessStatusCode();

            tokens = await Fixture.GetAntiforgeryTokensAsync(httpClient, TestContext.Current.CancellationToken);

            var approveFormData = new Dictionary<String, String>
            {
                [tokens.FormFieldName] = tokens.RequestToken,
                ["Scopes[0].IsSelected"] = "true",
                ["Scopes[0].Name"] = expectIdToken ? "openid" : "scope_2" 
            };
            var approveFormContent = new FormUrlEncodedContent(approveFormData);

            var approvePostResponse = await httpClient.PostAsync("/approve", approveFormContent, TestContext.Current.CancellationToken);
            approvePostResponse.EnsureRedirectStatusCode();

            var queryString = approvePostResponse.Headers.Location!.ToString()
                .Substring(approvePostResponse.Headers.Location!.ToString().IndexOf('?'));

            var queryParams = System.Web.HttpUtility.ParseQueryString(queryString);

            var code = queryParams["code"];
            Assert.NotNull(code);
            Assert.Equal(TestServerFixture.DefaultState, queryParams["state"]);
            #endregion

            #region Token
            var tokenRequest = new TokenRequest
            {
                ClientId = TestServerFixture.DefaultClientId,
                ClientSecret = TestServerFixture.DefaultClientSecret,
                RedirectUri = TestServerFixture.DefaultClientRedirectUri,
                GrantType = "authorization_code",
                Code = code,
                CodeVerifier = TestServerFixture.DefaultCodeVerifier
            };

            var tokenPostResponse = await httpClient.PostAsync("/token", tokenRequest.ToFormContent(), TestContext.Current.CancellationToken);
            tokenPostResponse.EnsureSuccessStatusCode();
            Assert.True(tokenPostResponse.Headers.CacheControl?.NoStore);

            var tokenResponse = await tokenPostResponse.Content.ReadFromJsonAsync<TokenResponse>(TestContext.Current.CancellationToken);
            Assert.NotNull(tokenResponse);
            Assert.NotNull(tokenResponse.AccessToken);
            Assert.NotNull(tokenResponse.RefreshToken);
            #endregion

            if (expectIdToken)
            {
                Assert.NotNull(tokenResponse.IdToken);
                await ValidateIdToken(httpClient, tokenResponse.IdToken);
                await ValidateAtHash(tokenResponse.AccessToken, tokenResponse.IdToken);
            }
            else
            {
                Assert.Null(tokenResponse.IdToken);
            }
        }

        private async Task ValidateIdToken(HttpClient httpClient, String idToken)
        {
            var jwksResponse = await httpClient.GetAsync("/.well-known/jwks.json", TestContext.Current.CancellationToken);
            jwksResponse.EnsureSuccessStatusCode();

            var jwks = await jwksResponse.Content.ReadFromJsonAsync<JwkSet>(TestContext.Current.CancellationToken);
            Assert.NotNull(jwks);

            var jwk = jwks.Jwks.First();
            byte[] n = jwk.Modulus.Base64UrlDecode();
            byte[] e = jwk.Exponent.Base64UrlDecode();

            using RSA rsaPublic = RSA.Create();
            rsaPublic.ImportParameters(new RSAParameters { Modulus = n, Exponent = e });

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = TestServerFixture.DefaultHost,
                ValidateAudience = true,
                ValidAudience = TestServerFixture.DefaultClientId,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(rsaPublic),
                ClockSkew = TimeSpan.FromMinutes(5)
            };

            var tokenHandler = new JsonWebTokenHandler();
            var result = await tokenHandler.ValidateTokenAsync(idToken, validationParameters);
            Assert.True(result.IsValid);
            Assert.Equal(TestServerFixture.DefaultNonce, result.Claims[JwtRegisteredClaimNames.Nonce]);
        }

        private async Task ValidateAtHash(String accessToken, String idToken)
        {
            using var scope = Fixture.Services.CreateScope();
            var atHash = scope.ServiceProvider.GetRequiredService<IComputeOpenIdAtHashService>()
                .Compute(accessToken);

            var tokenHandler = new JsonWebTokenHandler();
            var jsonToken = tokenHandler.ReadJsonWebToken(idToken);

            Assert.Equal(atHash, jsonToken.GetClaim(JwtRegisteredClaimNames.AtHash).Value);
        }

    }
}