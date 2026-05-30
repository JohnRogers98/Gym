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

        public RsaSecurityKeyProvider(IRsaKeyProvider _rsaKeyService, IConfiguration configuration)
        {
            _securityKey = new RsaSecurityKey(_rsaKeyService.GetRsa())
            {
                KeyId = configuration.GetRequiredConfiguration("Jwt:RsaKeyId"),
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
