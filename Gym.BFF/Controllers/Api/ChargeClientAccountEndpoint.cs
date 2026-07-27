using Gym.BFF.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Gym.BFF.Controllers.Api
{
    [ApiController]
    public class ChargeClientAccountEndpoint(IHttpClientFactory _httpClientFactory, IOptions<WebApiOptions> _webApiOptions) : ControllerBase
    {
        [HttpPost("api/clients/{clientId}/account/charge")]
        public async Task<IActionResult> HandleAsync(CancellationToken cancellationToken)
        {
            if (this.IsAccessTokenPresent() is false)
                return Unauthorized();

            var clientId = base.RouteData.Values["clientId"]?.ToString();
            if (String.IsNullOrEmpty(clientId))
                return BadRequest("clientId is required in path");

            using var adminApiClient = _httpClientFactory.CreateClient(_webApiOptions.Value.ClientName);
            using var proxyRequestMessage = await this.CreateProxyRequestAsync($"/api/clients/{clientId}/account/charge", cancellationToken: cancellationToken);
            using var response = await adminApiClient.SendAsync(proxyRequestMessage, cancellationToken);

            return await this.CreateProxyResponseAsync(response, cancellationToken);
        }
    }
}
