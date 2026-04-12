using Gym.Domain._Common;
using Gym.Domain.FormAuthContext.ValueObjects;

namespace Gym.Domain.FormAuthContext
{
    public interface IPasswordHashValidator
    {
        Result ValidateHash(HashedPassword hashedPassword, Password password);
    }
}
