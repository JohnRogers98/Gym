using Gym.AuthorizationServer.Client.Options;
using Gym.OAuth.Extensions;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Gym.AuthorizationServer.Client.Services
{
    public interface IRefreshTokenService
    {
        Task<HttpResult<TokenResponse>> HandleAsync(String refreshToken, String? resource = null, CancellationToken cancellationToken = default);
    }

    internal class RefreshTokenService(
        IHttpClientFactory _httpClientFactory,
        IOptions<ClientCredentialsOptions> _clientCredentialsOptions,
        IOptions<AuthorizationServerOptions> _authorizationServerOptions) : IRefreshTokenService
    {
        public async Task<HttpResult<TokenResponse>> HandleAsync(String refreshToken, String? resource = null, CancellationToken cancellationToken = default)
        {
            using HttpClient httpClient = _httpClientFactory.CreateClient(_authorizationServerOptions.Value.ClientName);

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, _authorizationServerOptions.Value.TokenEndpoint);

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", _clientCredentialsOptions.Value.GetCredentialsInBase64());

            TokenRequest tokenRequest = new()
            {
                GrantType = GrantTypes.RefreshToken,
                RedirectUri = _clientCredentialsOptions.Value.RedirectUri,
                Scope = _clientCredentialsOptions.Value.Scope,
                Resource = resource,
                RefreshToken = refreshToken
            };
            requestMessage.Content = tokenRequest.ToFormContent();

            var tokenResponse = await httpClient.SendAsync(requestMessage, cancellationToken);
            if (tokenResponse.IsSuccessStatusCode)
            {
                var tokenResponseObject = await tokenResponse.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken);
                if (tokenResponseObject is not null)
                    return HttpResult<TokenResponse>.Success(tokenResponseObject);
            }

            var errorResponse = await tokenResponse.Content.ReadFromJsonAsync<OAuthError>(cancellationToken);
            return HttpResult<TokenResponse>.Failure(errorResponse!);
        }
    }
}
