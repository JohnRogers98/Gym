using Gym.Domain.ClientContext.ValueObjects;

namespace Gym.Domain.ClientContext
{
    public interface IClientRepository
    {
        ClientId NextIdentity();
        Task SaveAsync(Client client, CancellationToken cancellationToken);
        Task<Client?> GetByIdAsync(ClientId id, CancellationToken cancellationToken);
        Task<Boolean> ExistsAsync(ClientId id, CancellationToken cancellationToken);
    }
}
