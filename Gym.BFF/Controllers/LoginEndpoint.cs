using Gym.AuthorizationServer.Client.Options;
using Gym.BFF.Options;
using Gym.BFF.Services;
using Gym.OAuth.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Gym.BFF.Controllers
{
    [ApiController]
    public class LoginEndpoint(
        ClientCredentialsOptions _clientCredentialsOptions,
        AuthorizationServerOptions _authorizationServerOptions,
        IOptions<ResourceUrisOptions> _resourceUrisOptions,
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
                ClientId = _clientCredentialsOptions.ClientId,
                ResponseType = "code",
                RedirectUri = _clientCredentialsOptions.RedirectUri,
                Scope = _clientCredentialsOptions.Scope,
                State = state,
                Nonce = nonce,
                CodeChallengeMethod = codeChallengePair.CodeChallengeMethod,
                CodeChallenge = codeChallengePair.CodeChallenge,
                Resource = _resourceUrisOptions.Value.Api
            };

            return base.Redirect($"{_authorizationServerOptions.FullAuthorizeUrl}{authorizeQuery.ToQueryString()}");
        }
    }

}
