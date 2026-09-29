using Gym.AuthorizationServer.Client.Options;
using Gym.BFF.Integration.Tests.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Gym.BFF.Integration.Tests.Tests
{
    [Collection<BFFServerCollection>]
    public class AccessTokenHandlerTests(BFFServerFixture _fixture, ITestOutputHelper _outputHelper) 
        : IntegrationTest(_fixture, _outputHelper)
    {
        [Fact]
        public async Task Request_With_Active_Access_Token()
        {
            var httpClient = Fixture.CreateClient();

            // Given
            String refreshToken = Guid.NewGuid().ToString();
            String accessToken = this.CreateJwtToken();
            await this.MakeLoginCallToSetupSesionCookieAsync(httpClient, accessToken, refreshToken);

            String endpoint = "/test-endpoint";
            String httpMethod = "GET";

            this.SetupProtectedResourceEndpointMock(endpoint, httpMethod, responseStatusCode: 200);           

            ProtectedResourceProxyRequest protectedResourceProxyRequest = new()
            {
                RelativeUrl = endpoint,
                Method = httpMethod,
                JsonSerializedBody = String.Empty
            };

            // When
            var response = await this.MakeFakeRequestToProtectedResource(httpClient, protectedResourceProxyRequest);

            // Then
            Assert.Equal(HttpStatusCode.OK, response?.StatusCode);

            var matchingLogs = Fixture.ProtectedResourceServerMock
                .FindLogEntries(Request.Create().WithPath(new PathString(endpoint)).UsingGet().WithHeader("Authorization", $"Bearer {accessToken}"));
            Assert.NotEmpty(matchingLogs);
        }

        [Fact]
        public async Task Request_With_Expired_Access_Token()
        {
            var httpClient = Fixture.CreateClient();

            // Given
            String refreshToken = Guid.NewGuid().ToString();
            String expiredAccessToken = this.CreateJwtToken(isExpired: true);
            String newActiveAccessToken = this.CreateJwtToken();
            String newRefreshToken = Guid.NewGuid().ToString();

            await this.MakeLoginCallToSetupSesionCookieAsync(httpClient, expiredAccessToken, refreshToken);
            this.SetupRefreshTokenExchangeMock(refreshToken, newActiveAccessToken, newRefreshToken);

            String endpoint = "/test-endpoint";
            String httpMethod = "GET";

            this.SetupProtectedResourceEndpointMock(endpoint, httpMethod, responseStatusCode: 200);

            ProtectedResourceProxyRequest protectedResourceProxyRequest = new()
            {
                RelativeUrl = endpoint,
                Method = httpMethod,
                JsonSerializedBody = String.Empty
            };

            // When
            var response = await this.MakeFakeRequestToProtectedResource(httpClient, protectedResourceProxyRequest);

            // Then
            Assert.Equal(HttpStatusCode.OK, response?.StatusCode);

            var protectedResourceMatchingLogs = Fixture.ProtectedResourceServerMock
                .FindLogEntries(Request.Create().WithPath(new PathString(endpoint)).UsingGet().WithHeader("Authorization", $"Bearer {newActiveAccessToken}"));
            Assert.NotEmpty(protectedResourceMatchingLogs);
            
            var authorizationServerMatchingLogs = Fixture.AuthorizationServerMock
                .FindLogEntries(Request.Create().WithPath(new PathString("/" + this.GetTokenUrl())).UsingPost());
            Assert.Equal(2, authorizationServerMatchingLogs.Count());
        }

        private async Task MakeLoginCallToSetupSesionCookieAsync(HttpClient httpClient, String accessToken, String refreshToken)
        {
            Fixture.AuthorizationServerMock.SetupExchangeCodeToken(this.GetTokenUrl(), accessToken, refreshToken);

            var loginResponse = await httpClient.GetAsync("/login", TestContext.Current.CancellationToken);
            var queryParams = QueryHelpers.ParseQuery(loginResponse.Headers.Location!.Query);
            String state = queryParams["state"]!;

            await httpClient.GetAsync($"/callback?code=test_code&state={state}", TestContext.Current.CancellationToken);
        }

        private void SetupRefreshTokenExchangeMock(String oldRefreshToken, String newAccessTokn, String newRefreshToken) =>
            Fixture.AuthorizationServerMock.SetupRefreshTokenExchangeMock(this.GetTokenUrl(), oldRefreshToken, newAccessTokn, newRefreshToken);

        private void SetupProtectedResourceEndpointMock(String endpoint, String? httpMethod = null, Int32 responseStatusCode = 200, String? responseBody = null)
        {
            var requestBuilder = Request.Create()
                .WithPath(new PathString(endpoint));

            requestBuilder = httpMethod?.ToLower() switch
            {
                "get" => requestBuilder.UsingGet(),
                "post" => requestBuilder.UsingPost(),
                _ => requestBuilder.UsingGet()
            };

            var responseBuilder = Response.Create()
                .WithStatusCode(responseStatusCode);

            if (!String.IsNullOrEmpty(responseBody))
            {
                responseBuilder.WithBody(responseBody);
            }

            Fixture.ProtectedResourceServerMock
                .Given(requestBuilder)
                .RespondWith(responseBuilder);
        }

        private async Task<HttpResponseMessage?> MakeFakeRequestToProtectedResource(HttpClient httpClient, ProtectedResourceProxyRequest protectedResourceProxyRequest)
        {
            String requestJson = JsonSerializer.Serialize(protectedResourceProxyRequest);

            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, FakeProtectedResourceProxyController.GetUri)
            {
                Content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json")
            };

            return await httpClient.SendAsync(httpRequest, TestContext.Current.CancellationToken);
        }

        private String CreateJwtToken(Boolean isExpired = false)
        {
            var claimsIdentity = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Iss, "test_issuer"),
                new Claim(JwtRegisteredClaimNames.Aud, "test_audience"),
                new Claim(JwtRegisteredClaimNames.Sub, "test_subject")
            ]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = isExpired ? DateTime.UtcNow.AddMinutes(-5) : DateTime.UtcNow.AddMinutes(5)
            };

            return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
        }

        private String GetTokenUrl() => Fixture.Services.GetRequiredOption<AuthorizationServerOptions>().TokenEndpoint;

    }
}
