using Gym.AuthorizationServer.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Gym.AuthorizationServer.Services.Rsa
{
    public interface IRsaKeyProvider
    {
        RSA GetRsa();
    }

    public class RsaKeyProvider : IRsaKeyProvider
    {
        private readonly RSA _rsa;

        public RsaKeyProvider(IOptions<JwtOptions> jwtOptions)
        {
            var keyPath = jwtOptions.Value.RsaKeyPath;

            var privateKeyPem = File.ReadAllText(keyPath);

            _rsa = RSA.Create();
            _rsa.ImportFromPem(privateKeyPem);
        }

        public RSA GetRsa() => _rsa;
    }
}
