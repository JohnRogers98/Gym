using Gym.AuthorizationServer.Client.Options;
using Gym.OAuth.Extensions;
using System.Net.Http.Json;

namespace Gym.AuthorizationServer.Client.Services
{
    public interface IJwkKeyProvider
    {
        Task<HttpResult<Jwk>> GetKeyAsync(CancellationToken cancellationToken = default);
    }

    internal class JwkKeyProvider(IHttpClientFactory _httpClientFactory, AuthorizationServerOptions _authorizationServerOptions) : IJwkKeyProvider
    {
        public async Task<HttpResult<Jwk>> GetKeyAsync(CancellationToken cancellationToken = default)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(_authorizationServerOptions.ClientName);

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, _authorizationServerOptions.JwksEndpoint);

            var jwksResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

            if (jwksResponse.IsSuccessStatusCode)
            {
                var jwks = await jwksResponse.Content.ReadFromJsonAsync<JwkSet>(cancellationToken);
                if (jwks is not null && jwks.Jwks.Any(jwk => jwk.KeyId == _authorizationServerOptions.Kid))
                {
                    return HttpResult<Jwk>.Success(jwks.Jwks.First(jwk => jwk.KeyId == _authorizationServerOptions.Kid));
                }
                return HttpResult<Jwk>.Failure($"No jwk with kid - {{{_authorizationServerOptions.Kid}}} found.");
            }

            var errorResonse = await jwksResponse.Content.ReadFromJsonAsync<OAuthError>(cancellationToken);
            return HttpResult<Jwk>.Failure(errorResonse!);
        }
    }
}
