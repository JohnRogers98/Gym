using Gym.Domain._Shared;

namespace Gym.Domain.ClientContext
{
    public interface IClientByUserIdFinder
    {
        Task<Client?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken);

        Task<Boolean> ExistsByUserIdAsync(UserId userId, CancellationToken cancellationToken);
    }
}
