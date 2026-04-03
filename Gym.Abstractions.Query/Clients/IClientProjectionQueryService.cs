namespace Gym.Abstractions.Query.Clients
{
    public interface IClientProjectionQueryService
    {
        Task<ClientProjection?> GetByIdAsync(String clientId, CancellationToken cancellationToken);

        Task<ClientProjection?> GetByUserIdAsync(String userId, CancellationToken cancellationToken);

        Task<IEnumerable<ClientProjection>> GetAllAsync(CancellationToken cancellationToken);
    }
}
