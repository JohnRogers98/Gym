using Gym.BFF.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Gym.BFF.Controllers.Api
{
    [ApiController]
    public class CancelCalendarEventEndpoint(IHttpClientFactory _httpClientFactory, IOptions<WebApiOptions> _webApiOptions) : ControllerBase
    {
        [HttpPost("api/admin-calendar-events/{calendarEventId}/cancel")]
        public async Task<IActionResult> HandleAsync(CancellationToken cancellationToken)
        {
            if (this.IsAccessTokenPresent() is false)
                return Unauthorized();

            var calendarEventId = base.RouteData.Values["calendarEventId"]?.ToString();
            if (String.IsNullOrEmpty(calendarEventId))
                return BadRequest("clientId is required in path");

            using var adminApiClient = _httpClientFactory.CreateClient(_webApiOptions.Value.ClientName);
            using var proxyRequestMessage = await this.CreateProxyRequestAsync($"/api/admin-calendar-events/{calendarEventId}/cancel", cancellationToken: cancellationToken);
            using var response = await adminApiClient.SendAsync(proxyRequestMessage, cancellationToken);

            return await this.CreateProxyResponseAsync(response, cancellationToken);
        }
    }
}
