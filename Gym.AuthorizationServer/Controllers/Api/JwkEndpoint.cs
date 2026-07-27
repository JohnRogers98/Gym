using Ardalis.ApiEndpoints;
using Gym.AuthorizationServer.Services.Rsa;
using Gym.OAuth.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Gym.AuthorizationServer.Controllers.Api
{
    [Route(".well-known/jwks.json")]
    [ApiController]
    public class JwkEndpoint(IRsaJwkService _rsaJwkService) : EndpointBaseAsync.WithoutRequest.WithActionResult<JwkSet>
    {
        [HttpGet]
        public override async Task<ActionResult<JwkSet>> HandleAsync(CancellationToken cancellationToken = default)
        {
            JwkSet jwks = new JwkSet()
            {
                Jwks = [_rsaJwkService.GetJwk()]
            };

            return base.Ok(jwks);
        }
    }
}
