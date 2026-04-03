using Gym.Domain.FormAuthContext.ValueObjects;

namespace Gym.Domain.FormAuthContext
{
    public interface IPasswordHasher
    {
        HashedPassword HashPassword(Password password);
    }
}
