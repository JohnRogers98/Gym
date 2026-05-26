using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Gym.AuthorizationServer.Services
{
    public interface IRsaSigningService
    {
        SigningCredentials GetSigningCredentials();
    }

    public class RsaSigningService : IRsaSigningService
    {
        private readonly SigningCredentials _credentials;

        public RsaSigningService(IConfiguration configuration)
        {
            var keyPath = configuration["Jwt:PrivateKeyPath"]
                ?? throw new InvalidOperationException("Jwt:PrivateKeyPath is not configured");

            var privateKeyPem = File.ReadAllText(keyPath);

            var rsa = RSA.Create();
            rsa.ImportFromPem(privateKeyPem);

            var securityKey = new RsaSecurityKey(rsa);
            _credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
        }

        public SigningCredentials GetSigningCredentials() => _credentials;
    }
}
