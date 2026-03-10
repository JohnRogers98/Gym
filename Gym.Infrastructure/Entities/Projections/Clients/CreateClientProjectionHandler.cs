using Gym.Abstractions.Query.Clients;
using Gym.Domain.ClientContext;
using Gym.Domain.ClientContext.Events;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.DtoDeserializers;
using Gym.Infrastructure.Entities.Extensions;
using Gym.Infrastructure.Entities.Repositories.Clients;
using Gym.Infrastructure.Entities.Repositories.Clients.EventsDto;
using Gym.Infrastructure.Entities.Repositories.Users;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.Clients
{
    internal class CreateClientProjectionHandler(
        IMongoCollection<ClientEntity> _clientCollection,
        IMongoCollection<UserEntity> _userCollection,
        IMongoCollection<ClientProjection> _clientProjections,
        MongoUnitOfWork _mongoUnitOfWork,
        IEventDtoDeserializer _eventDtoDeserializer) : IProjectionHandler
    {
        public Boolean CanHandle(String aggregateType, String operation)
        {
            return aggregateType == nameof(Client) && operation == nameof(ClientCreatedDomainEvent);
        }

        public async Task HandleAsync(EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var clientCreatedDto = _eventDtoDeserializer.Deserialize<ClientCreatedDto>(eventEntity);

            var clientEntity = await _clientCollection.Find(client => client.UserId == clientCreatedDto.UserId.ToObjectId())
                .FirstAsync(cancellationToken);

            var userEntity = await _userCollection
                .Find(user => user.Id == clientCreatedDto.UserId.ToObjectId())
                .FirstAsync(cancellationToken);

            var projection = new ClientProjection(
                Id: clientEntity.Id.ToString(),
                UserId: clientEntity.UserId.ToString(),
                Username: userEntity.TelegramUsername,
                FirstName: userEntity.FirstName,
                LastName: userEntity.LastName,
                AvailableTrainingsCount: 0
            );

            await _clientProjections.InsertOneAsync(_mongoUnitOfWork.Session, projection, cancellationToken: cancellationToken);
        }
    }
}
