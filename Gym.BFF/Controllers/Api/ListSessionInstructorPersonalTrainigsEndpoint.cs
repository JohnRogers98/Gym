using Gym.BFF.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Gym.BFF.Controllers.Api
{
    [ApiController]
    public class ListSessionInstructorPersonalTrainigsEndpoint(IHttpClientFactory _httpClientFactory, IOptions<WebApiOptions> _webApiOptioons) : ControllerBase
    {
        [HttpGet("api/instructors/me/personal-trainings")]
        public async Task<IActionResult> HandleAsync(CancellationToken cancellationToken)
        {
            if (this.IsAccessTokenPresent() is false)
                return Unauthorized();

            using var webApiClient = _httpClientFactory.CreateClient(_webApiOptioons.Value.ClientName);
            using var proxyRequestMessage = await this.CreateProxyRequestAsync("/api/instructors/me/personal-trainings", cancellationToken: cancellationToken);
            using var response = await webApiClient.SendAsync(proxyRequestMessage, cancellationToken);

            return await this.CreateProxyResponseAsync(response, cancellationToken);
        }
    }
}
