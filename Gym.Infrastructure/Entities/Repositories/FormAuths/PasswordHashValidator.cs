using Gym.Domain._Common;
using Gym.Domain.FormAuthContext;
using Gym.Domain.FormAuthContext.Errors;
using Gym.Domain.FormAuthContext.ValueObjects;
using Isopoh.Cryptography.Argon2;

namespace Gym.Infrastructure.Entities.Repositories.FormAuths
{
    internal class PasswordHashValidator : IPasswordHashValidator
    {
        public Result ValidateHash(HashedPassword hashedPassword, Password password)
        {
            if (Argon2.Verify(hashedPassword.Value, password.Value))
            {
                return Result.Ok();   
            }

            return Result.Fail(PasswordHashMatchError.Create());
        }
    }
}
