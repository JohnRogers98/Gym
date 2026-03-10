using Gym.Abstractions.Query.Clients;
using Gym.Domain.AccountContext;
using Gym.Domain.AccountContext.Events;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.DtoDeserializers;
using Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.Clients
{
    internal class AccountChargedProjectionHandler(
        IMongoCollection<ClientProjection> _clientProjections,
        MongoUnitOfWork _mongoUnitOfWork,
        IEventDtoDeserializer _eventDtoDeserializer) : IProjectionHandler
    {
        public Boolean CanHandle(String aggregateType, String operation)
        {
            return aggregateType == nameof(Account) && operation == nameof(AccountChargedDomainEvent);
        }

        public async Task HandleAsync(EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var accountChargedDto = _eventDtoDeserializer.Deserialize<AccountChargedDto>(eventEntity);

            await _clientProjections.UpdateOneAsync(
               _mongoUnitOfWork.Session,
               projection => projection.UserId == accountChargedDto.UserId,
               Builders<ClientProjection>.Update.Inc(x => x.AvailableTrainingsCount, accountChargedDto.ByCount),
               cancellationToken: cancellationToken);
        }
    }
}
