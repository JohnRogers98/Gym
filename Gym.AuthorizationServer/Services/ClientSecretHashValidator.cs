using Isopoh.Cryptography.Argon2;

namespace Gym.AuthorizationServer.Services
{
    public interface IClientSecretHashValidator
    {
        Boolean ValidateSecret(String secretHash, String secret);
    }
    public class ClientSecretHashValidator : IClientSecretHashValidator
    {
        public Boolean ValidateSecret(String secretHash, String secret)
          => Argon2.Verify(secretHash, secret);
    }
}
