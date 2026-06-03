using Gym.BFF.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Gym.BFF.Services.Jwks
{
    public interface IRsaSecurityKeyProvider
    {
        Task<RsaSecurityKey> GetKeyAsync(CancellationToken cancellationToken);
    }

    public class RsaSecurityKeyProvider(
        IHttpClientFactory _httpClientFactory,
        IOptions<UrlsOptions> _urls,
        IMemoryCache _cache) : IRsaSecurityKeyProvider
    {
        private readonly TimeSpan _cacheTtl = TimeSpan.FromHours(1);
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        public async Task<RsaSecurityKey> GetKeyAsync(CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue<RsaSecurityKey>(_urls.Value.AuthorizationServer.Kid, out var cachedKey))
                return cachedKey!;

            await _lock.WaitAsync(cancellationToken);
            try
            {
                // Double-check
                if (_cache.TryGetValue<RsaSecurityKey>(_urls.Value.AuthorizationServer.Kid, out cachedKey))
                    return cachedKey!;

                var fetchedJwt = await this.FetchJwkFromServerAsync(cancellationToken);

                var securityKey = this.CreateSecurityKey(fetchedJwt);
                _cache.Set(_urls.Value.AuthorizationServer.Kid, securityKey, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheTtl,
                    Priority = CacheItemPriority.NeverRemove
                });

                return securityKey;
            }
            finally { _lock.Release(); }
        }

        private async Task<Jwk> FetchJwkFromServerAsync(CancellationToken cancellationToken)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(HttpClientNames.AuthorizationServer);

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, _urls.Value.AuthorizationServer.Jwks);

            var jwksResponse = await httpClient.SendAsync(requestMessage, cancellationToken);
            jwksResponse.EnsureSuccessStatusCode();

            var jwks = await jwksResponse.Content.ReadFromJsonAsync<JwkSet>(cancellationToken);
            if (jwks is null || jwks.Jwks.Any(jwk => jwk.KeyId == _urls.Value.AuthorizationServer.Kid) is false)
                throw new InvalidOperationException($"Failed to load jwks with kid {_urls.Value.AuthorizationServer.Kid}");

            return jwks.Jwks.First(jwk => jwk.KeyId == _urls.Value.AuthorizationServer.Kid);
        }

        private RsaSecurityKey CreateSecurityKey(Jwk jwk)
        {
            var rsa = RSA.Create();
            rsa.ImportParameters(new RSAParameters
            {
                Modulus = jwk.Modulus.Base64UrlDecode(),
                Exponent = jwk.Exponent.Base64UrlDecode()
            });

            return new RsaSecurityKey(rsa)
            {
                CryptoProviderFactory = new CryptoProviderFactory
                {
                    CacheSignatureProviders = false
                }
            };
        }

    }
}
