using Gym.Application.Extensions;
using Gym.Domain._Shared;
using Gym.Domain.ClientContext;
using Gym.Domain.ClientContext.ValueObjects;
using Gym.Infrastructure.Entities.Extensions;
using Gym.Infrastructure.Entities.Extensions.Mappings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Repositories.Clients
{
    internal class ClientRepository(IMongoCollection<ClientEntity> _clientCollection, MongoUnitOfWork _mongoUnitOfWork) : IClientRepository, IClientByUserIdFinder
    {
        public ClientId NextIdentity() => ClientId.From(ObjectId.GenerateNewId().ToString()).Unwrap();

        public async Task<Boolean> ExistsByUserIdAsync(UserId userId, CancellationToken cancellationToken)
            => await _clientCollection.Find(_mongoUnitOfWork.Session, eClient => eClient.UserId == userId.Value.ToObjectId()).AnyAsync(cancellationToken);

        public async Task<Client?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken)
        {
            var foundedEntity = await _clientCollection.Find(_mongoUnitOfWork.Session, eClient => eClient.UserId == userId.Value.ToObjectId())
              .FirstOrDefaultAsync(cancellationToken);

            return foundedEntity?.ToDomain();
        }

        public async Task<Client?> GetByIdAsync(ClientId id, CancellationToken cancellationToken)
        {
            var foundedEntity = await _clientCollection.Find(_mongoUnitOfWork.Session, eClient => eClient.Id == id.Value.ToObjectId())
                .FirstOrDefaultAsync(cancellationToken);

            return foundedEntity?.ToDomain();
        }

        public async Task SaveAsync(Client client, CancellationToken cancellationToken)
        {
            ClientEntity clientEntity = client.ToEntity();

            await _clientCollection.ReplaceOneAsync(
                _mongoUnitOfWork.Session,
                eClient => eClient.Id == clientEntity.Id,
                clientEntity,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
        }

        public async Task<Boolean> ExistsAsync(ClientId id, CancellationToken cancellationToken) =>
            await _clientCollection.Find(_mongoUnitOfWork.Session, eClient => eClient.Id == id.Value.ToObjectId()).AnyAsync(cancellationToken);

    }
}
