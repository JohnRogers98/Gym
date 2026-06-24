using Gym.AuthorizationServer.Client.Services;
using Gym.BFF.Options;
using Gym.BFF.Services;
using Gym.BFF.Services.Session;
using Gym.OAuth.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Gym.BFF.Controllers
{
    [ApiController]
    public class TelegramInitEndpoint(
        IOptions<ResourceUrisOptions> _resourceUrisOptions,
        ITelegramAssertionService _telegramAssertionService,
        IOAuthIdTokenValidator _idTokenValidator,
        ISetTokensToClientSideSessionService _setTokensToClientSideSessionService) : ControllerBase
    {
        [HttpPost("telegram-init")]
        public async Task<IActionResult> HandleAsync([FromForm] String initData, CancellationToken cancellationToken)
        {
            if (initData is null)
                return BadRequest(new OAuthError() { Error = "invalid_request", ErrorDescription = "No init data present" });

            var tokenResponseResult = await _telegramAssertionService
                .HandleAsync(initData, _resourceUrisOptions.Value.Api, cancellationToken);
            if (tokenResponseResult.IsFailure)
                return BadRequest(tokenResponseResult.Error);

            if (tokenResponseResult.Value.IdToken is not null)
            {
                Result<ClaimsPrincipal> result = await _idTokenValidator
                    .ValidateAsync(tokenResponseResult.Value.IdToken, tokenResponseResult.Value.AccessToken, null, cancellationToken);
                if (result.IsFailed)
                    return BadRequest(new { error = result.ErrorCode, error_description = result.ErrorDescription });
            }

            await _setTokensToClientSideSessionService
                .HandleAsync(tokenResponseResult.Value.AccessToken, tokenResponseResult.Value.RefreshToken);

            return base.Ok();
        }
    }
}
