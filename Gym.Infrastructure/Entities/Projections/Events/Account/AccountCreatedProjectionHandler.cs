using Gym.Abstractions.Query.EventStore;
using Gym.Domain.AccountContext.Events;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.Deserializers;

namespace Gym.Infrastructure.Entities.Projections.Events.Account
{
    internal class AccountCreatedProjectionHandler(IEventDeserializer _eventDeserializer, EventProjectionStore _eventProjectionStore) : IProjectionHandler
    {
        public Boolean CanHandle(String aggregateType, String operation)
        {
            return aggregateType == nameof(Account) && operation == nameof(AccountCreatedDomainEvent);
        }

        public async Task HandleAsync(EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var accountCreatedDomainEvent = _eventDeserializer.Deserialize<AccountCreatedDomainEvent>(eventEntity);

            var projection = new EventProjection()
            {
                Id = eventEntity.Id,
                StreamId = eventEntity.StreamId,
                Operation = eventEntity.Operation,
                Version = eventEntity.Version,
                OccurredAt = eventEntity.OccurredAt,
                Payload = new()
            };

            await _eventProjectionStore.CreateAsync(projection, cancellationToken);
        }
    }
}
