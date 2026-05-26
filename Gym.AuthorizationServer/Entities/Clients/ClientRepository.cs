using MongoDB.Driver;

namespace Gym.AuthorizationServer.Entities.Clients
{
    public interface IClientRepository
    {
        Task<ClientEntity?> GetByIdAsync(String id, CancellationToken cancellationToken);
        Task AddAsync(ClientEntity entity, CancellationToken cancellationToken);
    }

    public class ClientRepository(IMongoCollection<ClientEntity> _clients) : IClientRepository
    {

        public async Task<ClientEntity?> GetByIdAsync(String id, CancellationToken cancellationToken)
        {
            return await _clients.Find(eClient => eClient.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(ClientEntity entity, CancellationToken cancellationToken)
        {
            await _clients.InsertOneAsync(entity,  cancellationToken: cancellationToken);
        }
    }
}
