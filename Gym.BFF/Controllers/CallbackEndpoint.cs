using Gym.BFF.Options;
using Gym.BFF.Services.Token;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;

namespace Gym.BFF.Controllers
{
    [ApiController]
    public class CallbackEndpoint(IHttpClientFactory _httpClientFactory, IOptions<ClientCredentialsOptions> _clientCredentialsOptions) : ControllerBase
    {
        [HttpGet("callback")]
        public async Task<IActionResult> Callback(
            [FromQuery] String code,
            [FromQuery] String? state,
            [FromQuery] String? error,
            [FromQuery] String? error_description,
            CancellationToken cancellationToken)
        {
            //TODO: redirect to SPA endpoint to properly handle error.
            if (!String.IsNullOrEmpty(error))
                return BadRequest(new {error, error_description});

            if (base.HttpContext.Session.ConsumeOAuthState() != state)
                return BadRequest(new { error = "invalid_state", error_description = "State mismatch" });

            HttpClient httpClient = _httpClientFactory.CreateClient("auth-server");
            
            var request = new HttpRequestMessage(HttpMethod.Post, "token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic",
                this.GetCredentialsInBase64(_clientCredentialsOptions.Value.ClientId, _clientCredentialsOptions.Value.ClientSecret));

            OAuthTokenRequest tokenRequest = new()
            {
                GrantType = "authorization_code",
                RedirectUri = _clientCredentialsOptions.Value.RedirectUri,
                Scope = _clientCredentialsOptions.Value.Scope,
                Code = code,
                CodeVerifier = base.HttpContext.Session.ConsumeOAuthCodeVerifier()
            };
            request.Content = tokenRequest.ToFormContent();

            var tokenResponse = await httpClient.SendAsync(request, cancellationToken);
            var tokenResponseObject = await tokenResponse.Content.ReadFromJsonAsync<OAuthTokenResponse>(cancellationToken);

            return base.Ok();
        }

        private String GetCredentialsInBase64(String login, String password)
            => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{login}:{password}"));
    }
}
