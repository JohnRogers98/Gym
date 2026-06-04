using Gym.BFF.Services.Session;
using Gym.BFF.Services.Token;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym.BFF.Controllers
{
    [ApiController]
    public class CallbackEndpoint(
        IOAuthExchangeCodeService _exchangeCodeService,
        IOAuthIdTokenValidator _idTokenValidator,
        ISetTokensToClientSideSessionService _setTokensToClientSideSessionService) : ControllerBase
    {
        //TODO: redirect to SPA endpoint to properly handle errors and success.
        [HttpGet("callback")]
        public async Task<IActionResult> Callback(
            [FromQuery] String code,
            [FromQuery] String? state,
            [FromQuery] String? error,
            [FromQuery] String? error_description,
            CancellationToken cancellationToken)
        {
            if (!String.IsNullOrEmpty(error))
                return BadRequest(new {error, error_description});

            if (String.IsNullOrEmpty(code))
                return BadRequest(new { error = "missing_code", error_description = "Code is missing" });

            var sessionState = base.HttpContext.Session.ConsumeOAuthState();
            var sessionNonce = base.HttpContext.Session.ConsumeOAuthNonce();
            var sessionCodeVerifier = base.HttpContext.Session.ConsumeOAuthCodeVerifier();

            if (sessionState != state)
                return BadRequest(new { error = "invalid_state", error_description = "State mismatch" });

            Result<OAuthTokenResponse> tokenResponseResult = await _exchangeCodeService
                .HandleAsync(code, sessionCodeVerifier, cancellationToken);

            if(tokenResponseResult.IsFailed)
                return BadRequest(new { error = "invalid_request", error_description = "Token request failed" });

            if (tokenResponseResult.Value.IdToken is not null)
            {
                Result<ClaimsPrincipal> result = await _idTokenValidator
                    .ValidateAsync(tokenResponseResult.Value.IdToken, tokenResponseResult.Value.AccessToken, sessionNonce, cancellationToken);
                if(result.IsFailed)
                    return BadRequest(new { error = result.ErrorCode, error_description = result.ErrorDescription });
            }

            await _setTokensToClientSideSessionService
                .HandleAsync(tokenResponseResult.Value.AccessToken, tokenResponseResult.Value.RefreshToken);

            return base.Ok();
        }
    }

}
