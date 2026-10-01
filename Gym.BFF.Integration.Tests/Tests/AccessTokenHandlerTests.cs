using Gym.BFF.Integration.Tests.Controllers;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Gym.BFF.Integration.Tests.Tests
{
    [Collection<BFFServerCollection>]
    public class AccessTokenHandlerTests(BFFServerFixture _fixture, ITestOutputHelper _outputHelper) 
        : ExtendedIntegrationTest(_fixture, _outputHelper)
    {
        [Fact]
        public async Task Request_With_Active_Access_Token()
        {
            using var httpClient = Fixture.CreateClient();

            // Given
            String refreshToken = Guid.NewGuid().ToString();
            String accessToken = base.CreateJwtToken();
            await base.MakeLoginCallToSetupSesionCookieAsync(httpClient, accessToken, refreshToken);

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
            using var response = await this.MakeFakeRequestToProtectedResource(httpClient, protectedResourceProxyRequest);

            // Then
            Assert.Equal(HttpStatusCode.OK, response?.StatusCode);

            var matchingLogs = Fixture.ProtectedResourceServerMock
                .FindLogEntries(Request.Create().WithPath(new PathString(endpoint)).UsingGet().WithHeader("Authorization", $"Bearer {accessToken}"));
            Assert.NotEmpty(matchingLogs);
        }

        [Fact]
        public async Task Request_With_Expired_Access_Token()
        {
            using var httpClient = Fixture.CreateClient();

            // Given
            String refreshToken = Guid.NewGuid().ToString();
            String expiredAccessToken = base.CreateJwtToken(isExpired: true);
            String newActiveAccessToken = base.CreateJwtToken();
            String newRefreshToken = Guid.NewGuid().ToString();

            await base.MakeLoginCallToSetupSesionCookieAsync(httpClient, expiredAccessToken, refreshToken);
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
            using var response = await this.MakeFakeRequestToProtectedResource(httpClient, protectedResourceProxyRequest);

            // Then
            Assert.Equal(HttpStatusCode.OK, response?.StatusCode);

            var protectedResourceMatchingLogs = Fixture.ProtectedResourceServerMock
                .FindLogEntries(Request.Create().WithPath(new PathString(endpoint)).UsingGet().WithHeader("Authorization", $"Bearer {newActiveAccessToken}"));
            Assert.NotEmpty(protectedResourceMatchingLogs);
            
            var authorizationServerMatchingLogs = Fixture.AuthorizationServerMock
                .FindLogEntries(Request.Create().WithPath(new PathString("/" + this.GetTokenUrl())).UsingPost());
            Assert.Equal(2, authorizationServerMatchingLogs.Count());
        }

        private void SetupRefreshTokenExchangeMock(String oldRefreshToken, String newAccessTokn, String newRefreshToken) =>
            Fixture.AuthorizationServerMock.SetupRefreshTokenExchangeMock(base.GetTokenUrl(), oldRefreshToken, newAccessTokn, newRefreshToken);

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

            using HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, FakeProtectedResourceProxyController.GetUri)
            {
                Content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json")
            };

            return await httpClient.SendAsync(httpRequest, TestContext.Current.CancellationToken);
        }
    }
}
