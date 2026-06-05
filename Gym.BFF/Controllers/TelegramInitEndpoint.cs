using Gym.BFF.Services.Session;
using Gym.BFF.Services.Token;
using Gym.OAuth.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym.BFF.Controllers
{
    [ApiController]
    public class TelegramInitEndpoint(
        IOAuthTelegramAssertionService _telegramAssertionService,
        IOAuthIdTokenValidator _idTokenValidator,
        ISetTokensToClientSideSessionService _setTokensToClientSideSessionService) : ControllerBase
    {
        //TODO: redirect to SPA endpoint to properly handle errors and success.
        [HttpPost("telegram-init")]
        public async Task<IActionResult> TelegramInit([FromForm] String initData, CancellationToken cancellationToken)
        {
            if (initData is null)
                return BadRequest(new { error = "invalid_request", error_description = "No init data present" });

            Result<TokenResponse> tokenResponseResult = await _telegramAssertionService
                .HandleAsync(initData, cancellationToken);
            if (tokenResponseResult.IsFailed)
                return BadRequest(new { error = "invalid_request", error_description = "Init data invalid" });

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
