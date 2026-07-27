using Microsoft.IdentityModel.Tokens;

namespace Gym.BFF.Integration.Tests.Rsa
{
    public class FakeRsaSecutiryKey(FakeRsaKeyProvider _rsaKeyProvider)
    {
        private RsaSecurityKey? _rsaSecurityKey;

        public RsaSecurityKey GetRsaSecurityKey()
        {
            if (_rsaSecurityKey is not null)
                return _rsaSecurityKey;

            _rsaSecurityKey = new RsaSecurityKey(_rsaKeyProvider.GetRsa())
            {
                KeyId = "gym_auth_key",
                /*CryptoProviderFactory = new CryptoProviderFactory
                {
                    CacheSignatureProviders = true
                }*/
            };
            return _rsaSecurityKey;
        }
    }
}
