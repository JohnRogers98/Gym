using Gym.BFF.Options;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Gym.BFF.Controllers.Api
{
    [ApiController]
    public class GetClientEndpoint(IHttpClientFactory _httpClientFactory, IOptions<WebApiOptions> _webApiOptioons) : ControllerBase
    {
        [HttpGet("api/clients")]
        public async Task<ActionResult<ListResponse<UserRoleDto>>> HandleAsync(CancellationToken cancellationToken)
        {
            if (this.IsAccessTokenPresent() is false)
                return Unauthorized();

            using var webApiClient = _httpClientFactory.CreateClient(_webApiOptioons.Value.ClientName);
            using var proxyRequestMessage = await this.CreateProxyRequestAsync("/api/clients", cancellationToken: cancellationToken);
            using var response = await webApiClient.SendAsync(proxyRequestMessage, cancellationToken);

            return await this.CreateProxyResponseAsync(response, cancellationToken);
        }
    }
}
