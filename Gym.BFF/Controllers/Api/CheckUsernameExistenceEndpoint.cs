using Gym.BFF.Options;
using Gym.WebDto.Responses.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Gym.BFF.Controllers.Api
{
    [ApiController]
    public class CheckUsernameExistenceEndpoint(IHttpClientFactory _httpClientFactory, IOptions<AuthorizationServerAdminApiOptions> _adminApiOptions) : ControllerBase
    {
        [HttpPost("api/users/check-username")]
        public async Task<ActionResult<CreateUserResponse>> HandleAsync(CancellationToken cancellationToken)
        {
            if (this.IsAccessTokenPresent() is false)
                return Unauthorized();

            using var adminApiClient = _httpClientFactory.CreateClient(_adminApiOptions.Value.ClientName);
            using var proxyRequestMessage = await this.CreateProxyRequestAsync("/api/users/check-username", cancellationToken: cancellationToken);
            using var response = await adminApiClient.SendAsync(proxyRequestMessage, cancellationToken);

            return await this.CreateProxyResponseAsync(response, cancellationToken);
        }
    }
}
