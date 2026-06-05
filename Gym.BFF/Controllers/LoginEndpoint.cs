using Gym.BFF.Options;
using Gym.BFF.Services;
using Gym.OAuth.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Gym.BFF.Controllers
{
    [ApiController]
    public class LoginEndpoint(
        IOptions<ClientCredentialsOptions> _clientCredentialsOptions,
        IOptions<UrlsOptions> _urlsOptions,
        IOAuthStateGenerator _stateGenerator,
        IOAuthNonceGenerator _nonceGenerator,
        ICodeChallengePairGenerator _codeChallengePairGenerator) : ControllerBase
    {
        [HttpGet("login")]
        public async Task<IActionResult> Login()
        {
            String state = _stateGenerator.Generate();
            base.HttpContext.Session.SetOAuthState(state);

            String nonce = _nonceGenerator.Generate();
            base.HttpContext.Session.SetOAuthNonce(nonce);

            CodeChallengePair codeChallengePair = _codeChallengePairGenerator.Generate();
            base.HttpContext.Session.SetOAuthCodeVerifier(codeChallengePair.CodeVerifier);

            AuthorizeQuery authorizeQuery = new()
            {
                ClientId = _clientCredentialsOptions.Value.ClientId,
                ResponseType = "code",
                RedirectUri = _clientCredentialsOptions.Value.RedirectUri,
                Scope = _clientCredentialsOptions.Value.Scope,
                State = state,
                Nonce = nonce,
                CodeChallengeMethod = codeChallengePair.CodeChallengeMethod,
                CodeChallenge = codeChallengePair.CodeChallenge
            };

            return base.Redirect($"{_urlsOptions.Value.AuthorizationServer.FullAuthorizeUrl}{authorizeQuery.ToQueryString()}");
        }
    }

}
