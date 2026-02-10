using Gym.Domain._Shared;

namespace Gym.Domain.ClientContext
{
    public interface IClientQueryService
    {
        Task<Boolean> ExistsByUserIdAsync(UserId userId, CancellationToken cancellationToken);
        Task<Client?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken);

        Task<Client?> GetByIdAsync(ClientId id, CancellationToken cancellationToken);
    }
}
