using System.Security.Cryptography;

namespace Gym.AuthorizationServer.Services.Rsa
{
    public interface IRsaKeyService
    {
        RSA GetRsa();
    }

    public class RsaKeyService : IRsaKeyService
    {
        private readonly RSA _rsa;

        public RsaKeyService(IConfiguration configuration)
        {
            var keyPath = configuration.GetRequiredConfiguration("Jwt:RsaKeyPath");

            var privateKeyPem = File.ReadAllText(keyPath);

            _rsa = RSA.Create();
            _rsa.ImportFromPem(privateKeyPem);
        }

        public RSA GetRsa() => _rsa;
    }
}
