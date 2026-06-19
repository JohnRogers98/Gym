using Isopoh.Cryptography.Argon2;

namespace Gym.AuthorizationServer.Infrastructure.Services
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
