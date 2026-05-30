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

        public RsaKeyProvider(IConfiguration configuration)
        {
            var keyPath = configuration.GetRequiredConfiguration("Jwt:RsaKeyPath");

            var privateKeyPem = File.ReadAllText(keyPath);

            _rsa = RSA.Create();
            _rsa.ImportFromPem(privateKeyPem);
        }

        public RSA GetRsa() => _rsa;
    }
}
