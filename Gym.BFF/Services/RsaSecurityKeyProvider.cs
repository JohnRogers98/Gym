using Gym.AuthorizationServer.Client.Options;
using Gym.AuthorizationServer.Client.Services;
using Gym.OAuth.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Gym.BFF.Services
{
    public interface IRsaSecurityKeyProvider
    {
        Task<RsaSecurityKey> GetKeyAsync(CancellationToken cancellationToken);
        Task<RsaSecurityKey> GetKeyAsync(String kid, CancellationToken cancellationToken);
    }

    public class RsaSecurityKeyProvider(
        IJwkKeyProvider _jwkKeyProvider,
        AuthorizationServerOptions _authorizationServerOptions,
        IMemoryCache _cache) : IRsaSecurityKeyProvider
    {
        private readonly TimeSpan _cacheTtl = TimeSpan.FromHours(1);
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        public async Task<RsaSecurityKey> GetKeyAsync(CancellationToken cancellationToken)
        {
            return await this.GetKeyAsync(_authorizationServerOptions.Kid, cancellationToken);
        }

        public async Task<RsaSecurityKey> GetKeyAsync(String kid, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue<RsaSecurityKey>(kid, out var cachedKey))
                return cachedKey!;

            await _lock.WaitAsync(cancellationToken);
            try
            {
                // Double-check
                if (_cache.TryGetValue<RsaSecurityKey>(kid, out cachedKey))
                    return cachedKey!;

                var fetchedJwk = await _jwkKeyProvider.GetKeyAsync(cancellationToken);

                var securityKey = this.CreateSecurityKey(fetchedJwk.Value);
                _cache.Set(kid, securityKey, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheTtl,
                    Priority = CacheItemPriority.NeverRemove
                });

                return securityKey;
            }
            finally { _lock.Release(); }
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
