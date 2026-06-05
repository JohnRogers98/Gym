using Gym.BFF.Options;
using Gym.OAuth.Extensions;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace Gym.BFF.Services.Token
{
    public interface IOAuthExchangeCodeService
    {
        Task<Result<TokenResponse>> HandleAsync(String code, String? codeVerifier, CancellationToken cancellationToken);
    }

    public class OAuthExchangeCodeService(
        IHttpClientFactory _httpClientFactory,
        IOptions<ClientCredentialsOptions> _clientCredentials,
        IOptions<UrlsOptions> _urls) : IOAuthExchangeCodeService
    {
        //TODO: propagate token error 
        public async Task<Result<TokenResponse>> HandleAsync(String code, String? codeVerifier, CancellationToken cancellationToken)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(HttpClientNames.AuthorizationServer);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, _urls.Value.AuthorizationServer.TokenEndpoint);

            requestMessage.Headers.Authorization 
                = new AuthenticationHeaderValue("Basic", _clientCredentials.Value.GetCredentialsInBase64());

            TokenRequest tokenRequest = new()
            {
                GrantType = GrantTypes.AuthorizationCode,
                RedirectUri = _clientCredentials.Value.RedirectUri,
                Scope = _clientCredentials.Value.Scope,
                Code = code,
                CodeVerifier = codeVerifier
            };
            requestMessage.Content = tokenRequest.ToFormContent();

            var tokenResponse = await httpClient.SendAsync(requestMessage, cancellationToken);
            tokenResponse.EnsureSuccessStatusCode();

            var deserializedResponse = await tokenResponse.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken)
                ?? throw new InvalidOperationException("Failed to deserialize token response");

            return Result<TokenResponse>.Success(deserializedResponse);
        }
    }
}
