using Gym.Domain._Shared;

namespace Gym.Domain.FormAuthContext
{
    public interface IFormAuthByUserIdFinder
    {
        Task<FormAuth?> GetFormAuthByUserIdAsync(UserId userId, CancellationToken cancellationToken);
    }
}
