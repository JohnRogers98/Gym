using Gym.BFF.Integration.Tests.Controllers;
using System.Net.Http.Headers;

namespace Gym.BFF.Integration.Tests.Tests
{
    [Collection<BFFServerCollection>]
    public class HeaderTests(BFFServerFixture _fixture, ITestOutputHelper _outputHelper) : IntegrationTest(_fixture, _outputHelper)
    {
        [Fact]
        public async Task Request_Without_X_Static_Header()
        {
            var httpClient = Fixture.CreateClient();
            var response = await httpClient.GetAsync(MissingStaticHeaderController.GetUri, TestContext.Current.CancellationToken);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Request_With_X_Static_Header()
        {
            var httpClient = Fixture.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, MissingStaticHeaderController.GetUri);
            request.Headers.AddXStaticHeader();

            var response = await httpClient.SendAsync(request, TestContext.Current.CancellationToken);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
    }
}
