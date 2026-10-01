using Gym.BFF.Integration.Tests.Controllers;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using static Gym.BFF.Integration.Tests.Controllers.RandomGuidGeneratorController;

namespace Gym.BFF.Integration.Tests.Tests
{
    [Collection<BFFServerCollection>]
    public class IdempotencyMiddlewareTests(BFFServerFixture _fixture, ITestOutputHelper _outputHelper)
        : ExtendedIntegrationTest(_fixture, _outputHelper)
    {
        [Fact]
        public async Task Request_With_Idempotent_Header_Key()
        {
            // Given
            var httpClient = Fixture.CreateClient();
            await base.MakeLoginCallToSetupSesionCookieAsync(httpClient);
            var idempotencyKey = Guid.NewGuid().ToString();

            // When

            // First request to generate a random GUID
            using HttpRequestMessage firstRequest = new (HttpMethod.Post, RandomGuidGeneratorController.GetUri);
            firstRequest.Headers.Add("Idempotency-Key", idempotencyKey);

            using var firstResponse = await httpClient.SendAsync(firstRequest, TestContext.Current.CancellationToken);
            firstResponse.EnsureSuccessStatusCode();
            var firstDeserializedGuid = await firstResponse.Content.ReadFromJsonAsync<RandomGuidResponse>(TestContext.Current.CancellationToken);

            // Second request with the same Idempotency-Key
            using HttpRequestMessage secondRequest = new (HttpMethod.Post, RandomGuidGeneratorController.GetUri);
            secondRequest.Headers.Add("Idempotency-Key", idempotencyKey);

            using var secondResponse = await httpClient.SendAsync(secondRequest, TestContext.Current.CancellationToken);
            secondResponse.EnsureSuccessStatusCode();
            var secondDeserializedGuid = await secondResponse.Content.ReadFromJsonAsync<RandomGuidResponse>(TestContext.Current.CancellationToken);

            // Then
            Assert.NotNull(firstDeserializedGuid);
            Assert.NotNull(secondDeserializedGuid);
            Assert.Equal(firstDeserializedGuid.Value, secondDeserializedGuid.Value);
        }
    }
}
