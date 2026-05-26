using Isopoh.Cryptography.Argon2;

namespace Gym.AuthorizationServer.Services
{
    public interface IPasswordHasher
    {
        String HashPassword(String pssword);
    }

    public class PasswordHasher : IPasswordHasher
    {
        public String HashPassword(String password) => Argon2.Hash(password);
    }
}
