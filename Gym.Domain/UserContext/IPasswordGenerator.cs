using Gym.Domain.FormAuthContext.ValueObjects;

namespace Gym.Domain.UserContext
{
    public interface IPasswordGenerator
    {
        Password Generate();
    }
}
