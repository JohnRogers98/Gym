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
    public class CallbackEndpoint(
        IOptions<ResourceUrisOptions> _resourceUrisOptions,
        IOptions<SpaOptions> _spaOptions,
        IExchangeCodeForTokenService _exchangeCodeForTokenService,
        IOAuthIdTokenValidator _idTokenValidator,
        ISetTokensToClientSideSessionService _setTokensToClientSideSessionService) : ControllerBase
    {
        //TODO: redirect to SPA endpoint to properly handle errors.
        [HttpGet("callback")]
        public async Task<IActionResult> HandleAsync(
            [FromQuery] String code,
            [FromQuery] String? state,
            [FromQuery] String? error,
            [FromQuery] String? error_description,
            CancellationToken cancellationToken)
        {
            if (!String.IsNullOrEmpty(error))
                return BadRequest(new {error, error_description});

            if (String.IsNullOrEmpty(code))
                return BadRequest(new OAuthError { Error = "missing_code", ErrorDescription = "Code is missing" });

            var sessionState = base.HttpContext.Session.ConsumeOAuthState();
            var sessionNonce = base.HttpContext.Session.ConsumeOAuthNonce();
            var sessionCodeVerifier = base.HttpContext.Session.ConsumeOAuthCodeVerifier();

            if (sessionState != state)
                return BadRequest(new OAuthError { Error = "invalid_state", ErrorDescription = "State mismatch" });

            var tokenResponseResult = await _exchangeCodeForTokenService
                .HandleAsync(code, sessionCodeVerifier, _resourceUrisOptions.Value.Api, cancellationToken);

            if(tokenResponseResult.IsFailure)
                return BadRequest(tokenResponseResult.Error);

            if (tokenResponseResult.Value.IdToken is not null)
            {
                Result<ClaimsPrincipal> result = await _idTokenValidator
                    .ValidateAsync(tokenResponseResult.Value.IdToken, tokenResponseResult.Value.AccessToken, sessionNonce, cancellationToken);
                if(result.IsFailed)
                    return BadRequest(new OAuthError { Error = result.ErrorCode, ErrorDescription = result.ErrorDescription });
            }

            await _setTokensToClientSideSessionService
                .HandleAsync(tokenResponseResult.Value.AccessToken, tokenResponseResult.Value.RefreshToken, tokenResponseResult.Value.IdToken);

            return base.Redirect(_spaOptions.Value.FullCallbackPath);
        }
    }
}
