using Gym.BFF.Options;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Gym.BFF.Controllers.Api
{
    [ApiController]
    public class ListUserRolesEndpoint(IOptions<AuthorizationServerAdminApiOptions> _adminApiOptions, IHttpClientFactory _httpClientFactory) : ControllerBase
    {
        [HttpGet("api/user-roles")]
        public async Task<ActionResult<ListResponse<UserRoleDto>>> HandleAsync(CancellationToken cancellationToken)
        {
            if (this.IsAccessTokenPresent() is false)
                return Unauthorized();

            var adminApiClient = _httpClientFactory.CreateClient(_adminApiOptions.Value.ClientName);
            var proxyRequestMessage = await this.CreateProxyRequestAsync("/api/user-roles", cancellationToken: cancellationToken);
            var response = await adminApiClient.SendAsync(proxyRequestMessage, cancellationToken);

            return await this.CreateProxyResponseAsync(response, cancellationToken);
        }
    }
}
