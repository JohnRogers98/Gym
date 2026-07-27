using Gym.AuthorizationServer.Client.Options;
using Gym.OAuth.Extensions;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Gym.AuthorizationServer.Client.Services
{
    public interface IJwkKeyProvider
    {
        Task<HttpResult<Jwk>> GetKeyAsync(CancellationToken cancellationToken = default);
    }

    internal class JwkKeyProvider(IHttpClientFactory _httpClientFactory, IOptions<AuthorizationServerOptions> _authorizationServerOptions) : IJwkKeyProvider
    {
        public async Task<HttpResult<Jwk>> GetKeyAsync(CancellationToken cancellationToken = default)
        {
            using HttpClient httpClient = _httpClientFactory.CreateClient(_authorizationServerOptions.Value.ClientName);

            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, _authorizationServerOptions.Value.JwksEndpoint);

            var jwksResponse = await httpClient.SendAsync(requestMessage, cancellationToken);

            if (jwksResponse.IsSuccessStatusCode)
            {
                var jwks = await jwksResponse.Content.ReadFromJsonAsync<JwkSet>(cancellationToken);
                if (jwks is not null && jwks.Jwks.Any(jwk => jwk.KeyId == _authorizationServerOptions.Value.Kid))
                {
                    return HttpResult<Jwk>.Success(jwks.Jwks.First(jwk => jwk.KeyId == _authorizationServerOptions.Value.Kid));
                }
                return HttpResult<Jwk>.Failure($"No jwk with kid - {{{_authorizationServerOptions.Value.Kid}}} found.");
            }

            var errorResonse = await jwksResponse.Content.ReadFromJsonAsync<OAuthError>(cancellationToken);
            return HttpResult<Jwk>.Failure(errorResonse!);
        }
    }
}
