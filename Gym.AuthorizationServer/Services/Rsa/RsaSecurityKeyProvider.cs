using Gym.AuthorizationServer.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Gym.AuthorizationServer.Services.Rsa
{
    public interface IRsaSecurityKeyProvider
    {
        RsaSecurityKey GetRsaSecurityKey();
    }

    public class RsaSecurityKeyProvider : IRsaSecurityKeyProvider
    {
        private readonly RsaSecurityKey _securityKey;

        public RsaSecurityKeyProvider(IRsaKeyProvider _rsaKeyService, IOptions<JwtOptions> jwtOptions)
        {
            _securityKey = new RsaSecurityKey(_rsaKeyService.GetRsa())
            {
                KeyId = jwtOptions.Value.RsaKeyId,
                /*CryptoProviderFactory = new CryptoProviderFactory
                {
                    CacheSignatureProviders = true
                }*/
            };
        }

        public RsaSecurityKey GetRsaSecurityKey()
        {
            return _securityKey;
        }
    }
}
