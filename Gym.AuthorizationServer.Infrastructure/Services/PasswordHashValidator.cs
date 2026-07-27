using Isopoh.Cryptography.Argon2;

namespace Gym.AuthorizationServer.Infrastructure.Services
{
    public interface IPasswordHashValidator
    {
        Boolean ValidatePassword(String passwordHash, String password);
    }

    public class PasswordHashValidator : IPasswordHashValidator
    {
        public Boolean ValidatePassword(String passwordHash, String password) 
            => Argon2.Verify(passwordHash, password);
    }
}
