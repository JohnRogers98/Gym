using Gym.AuthorizationServer.Client.Options;
using Gym.OAuth.Extensions;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Gym.AuthorizationServer.Client.Services
{
    public interface ITelegramAssertionService
    {
        Task<HttpResult<TokenResponse>> HandleAsync(String initData, String resource, CancellationToken cancellationToken = default);
    }

    internal class TelegramAssertionService(
        IHttpClientFactory _httpClientFactory,
        ClientCredentialsOptions _clientCredentials,
        AuthorizationServerOptions _authorizationServerOptions) : ITelegramAssertionService
    {
        public async Task<HttpResult<TokenResponse>> HandleAsync(String initData, String resource, CancellationToken cancellationToken = default)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(_authorizationServerOptions.ClientName);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, _authorizationServerOptions.TokenEndpoint);

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", _clientCredentials.GetCredentialsInBase64());

            TokenRequest tokenRequest = new()
            {
                GrantType = GrantTypes.TelegramAssertion,
                RedirectUri = _clientCredentials.RedirectUri,
                Scope = _clientCredentials.Scope,
                Resource = resource,
                Assertion = initData
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
