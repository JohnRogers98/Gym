using Gym.BFF.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Gym.BFF.Controllers.Api
{

    [ApiController]
    public class CreateCalendarEventEndpoint(IHttpClientFactory _httpClientFactory, IOptions<WebApiOptions> _webApiOptions) : ControllerBase
    {
        [HttpPost("api/admin-calendar-events")]
        public async Task<IActionResult> HandleAsync(CancellationToken cancellationToken)
        {
            if (this.IsAccessTokenPresent() is false)
                return Unauthorized();

            using var adminApiClient = _httpClientFactory.CreateClient(_webApiOptions.Value.ClientName);
            using var proxyRequestMessage = await this.CreateProxyRequestAsync("api/admin-calendar-events", true, cancellationToken: cancellationToken);
            using var response = await adminApiClient.SendAsync(proxyRequestMessage, cancellationToken);

            return await this.CreateProxyResponseAsync(response, cancellationToken);
        }
    }
}

