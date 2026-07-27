using Gym.AuthorizationServer.Infrastructure.Session;
using MongoDB.Driver;

namespace Gym.AuthorizationServer.Infrastructure.Entities.Clients
{
    public interface IClientRepository
    {
        Task<ClientEntity?> GetByIdAsync(String id, CancellationToken cancellationToken);
        Task AddAsync(ClientEntity entity, CancellationToken cancellationToken);
    }

    public class ClientRepository(IMongoCollection<ClientEntity> _clients, MongoUnitOfWork _mongoUnitOfWork) : IClientRepository
    {

        public async Task<ClientEntity?> GetByIdAsync(String id, CancellationToken cancellationToken)
        {
            return await _clients.Find(_mongoUnitOfWork.Session, eClient => eClient.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(ClientEntity entity, CancellationToken cancellationToken)
        {
            await _clients.InsertOneAsync(_mongoUnitOfWork.Session, entity, cancellationToken: cancellationToken);
        }
    }
}
