using System.Net.Http.Json;

namespace Gym.AuthorizationServer.Integration.Tests.Middleware
{
    [Collection<TestServerCollection>]
    public class XForwardedHeadersTests(TestServerFixture _fixture, ITestOutputHelper _outputHelper) : IntegrationTest(_fixture, _outputHelper)
    {
        [Fact]
        public async Task Verify_That_X_Forwarded_Headers_Applied()
        {
            //Given
            var client = Fixture.CreateClient();
            var forwardedHost = "api.example.com";
            var forwardedScheme = "https";

            var request = new HttpRequestMessage(HttpMethod.Get, ServerInfoController.GetInfoUri);
            request.Headers.Add("X-Forwarded-Host", forwardedHost);
            request.Headers.Add("X-Forwarded-Proto", forwardedScheme);

            //When
            var serverInfoGetResponse = await client.SendAsync(request, TestContext.Current.CancellationToken);
            var serverInfoObject = await serverInfoGetResponse.Content.ReadFromJsonAsync<ServerInfo>(TestContext.Current.CancellationToken);

            //Then
            Assert.NotNull(serverInfoObject);
            Assert.Equal(forwardedHost, serverInfoObject.Host);
            Assert.Equal(forwardedScheme, serverInfoObject.Scheme);
        }
    }
}
