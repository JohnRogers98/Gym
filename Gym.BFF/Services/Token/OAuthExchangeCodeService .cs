using Gym.BFF.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;

namespace Gym.BFF.Services.Token
{
    public interface IOAuthExchangeCodeService
    {
        Task<Result<OAuthTokenResponse>> HandleAsync(String code, String? codeVerifier, CancellationToken cancellationToken);
    }

    public class OAuthExchangeCodeService(
        IHttpClientFactory _httpClientFactory,
        IOptions<ClientCredentialsOptions> _clientCredentials,
        IOptions<UrlsOptions> _urls) : IOAuthExchangeCodeService
    {
        //TODO: propagate token error 
        public async Task<Result<OAuthTokenResponse>> HandleAsync(String code, String? codeVerifier, CancellationToken cancellationToken)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(HttpClientNames.AuthorizationServer);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, _urls.Value.AuthorizationServer.TokenEndpoint);

            requestMessage.Headers.Authorization 
                = new AuthenticationHeaderValue("Basic", this.GetCredentialsInBase64(_clientCredentials.Value.ClientId, _clientCredentials.Value.ClientSecret));

            OAuthTokenRequest tokenRequest = new()
            {
                GrantType = "authorization_code",
                RedirectUri = _clientCredentials.Value.RedirectUri,
                Scope = _clientCredentials.Value.Scope,
                Code = code,
                CodeVerifier = codeVerifier
            };
            requestMessage.Content = tokenRequest.ToFormContent();

            var tokenResponse = await httpClient.SendAsync(requestMessage, cancellationToken);
            tokenResponse.EnsureSuccessStatusCode();

            var deserializedResponse = await tokenResponse.Content.ReadFromJsonAsync<OAuthTokenResponse>(cancellationToken)
                ?? throw new InvalidOperationException("Failed to deserialize token response");

            return Result<OAuthTokenResponse>.Success(deserializedResponse);
        }

        private String GetCredentialsInBase64(String login, String password)
            => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{login}:{password}"));
    }
}
