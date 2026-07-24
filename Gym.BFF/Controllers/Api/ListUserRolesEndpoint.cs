using Gym.BFF.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Gym.BFF.Controllers.Api
{
    [ApiController]
    public class ListUserRolesEndpoint(IHttpClientFactory _httpClientFactory, IOptions<AuthorizationServerAdminApiOptions> _adminApiOptions) : ControllerBase
    {
        [HttpGet("api/user-roles")]
        public async Task<IActionResult> HandleAsync(CancellationToken cancellationToken)
        {
            if (this.IsAccessTokenPresent() is false)
                return Unauthorized();

            using var adminApiClient = _httpClientFactory.CreateClient(_adminApiOptions.Value.ClientName);
            using var proxyRequestMessage = await this.CreateProxyRequestAsync("/api/user-roles", cancellationToken: cancellationToken);
            using var response = await adminApiClient.SendAsync(proxyRequestMessage, cancellationToken);

            return await this.CreateProxyResponseAsync(response, cancellationToken);
        }
    }
}
