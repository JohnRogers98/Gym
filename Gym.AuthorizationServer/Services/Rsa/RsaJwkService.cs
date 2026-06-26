using Gym.AuthorizationServer.Options;
using Gym.OAuth.Extensions;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Gym.AuthorizationServer.Services.Rsa
{
    public interface IRsaJwkService
    {
        Jwk GetJwk();
    }

    public class RsaJwkService : IRsaJwkService
    {
        private readonly Jwk _jwk;

        public RsaJwkService(IRsaKeyProvider rsaKeyService, IOptions<JwtOptions> jwtOptions)
        {
            var keyId = jwtOptions.Value.RsaKeyId;

            RSAParameters publicParams = rsaKeyService.GetRsa().ExportParameters(false);
            String n = Convert.ToBase64String(publicParams.Modulus!).ToUrlSafe();
            String e = Convert.ToBase64String(publicParams.Exponent!).ToUrlSafe();

            _jwk = new Jwk()
            {
                KeyType = "RSA",
                PublicKeyUse = "sig",
                Algorithm = "RS256",
                KeyId = keyId,
                Modulus = n,
                Exponent = e
            };
        }

        public Jwk GetJwk() => _jwk;
    }
}
