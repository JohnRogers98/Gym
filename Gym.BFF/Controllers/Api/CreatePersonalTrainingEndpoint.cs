using Gym.BFF.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Gym.BFF.Controllers.Api
{
    [ApiController]
    public class CreatePersonalTrainingEndpoint(IHttpClientFactory _httpClientFactory, IOptions<WebApiOptions> _webApiOptioons) : ControllerBase
    {
        [HttpPost("api/personal-trainings")]
        public async Task<IActionResult> HandleAsync(CancellationToken cancellationToken)
        {
            if (this.IsAccessTokenPresent() is false)
                return Unauthorized();

            using var webApiClient = _httpClientFactory.CreateClient(_webApiOptioons.Value.ClientName);
            using var proxyRequestMessage = await this.CreateProxyRequestAsync("/api/personal-trainings", cancellationToken: cancellationToken);
            using var response = await webApiClient.SendAsync(proxyRequestMessage, cancellationToken);

            return await this.CreateProxyResponseAsync(response, cancellationToken);
        }
    }
}
