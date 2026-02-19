using Gym.Abstractions.Query.EventStore;
using Gym.Domain.AccountContext.Events;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.Deserializers;
using Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto;

namespace Gym.Infrastructure.Entities.Projections.Events.Account
{
    internal class AccountChargedProjectionHandler(IEventDeserializer _eventDeserializer, EventProjectionStore _eventProjectionStore) : IProjectionHandler
    {
        public Boolean CanHandle(String aggregateType, String operation)
        {
            return aggregateType == nameof(Account) && operation == nameof(AccountChargedDomainEvent);
        }

        public async Task HandleAsync(EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var accountChargedDomainEvent = _eventDeserializer.Deserialize<AccountChargedDomainEvent>(eventEntity);

            var projection = new EventProjection()
            {
                Id = eventEntity.Id,
                StreamId = eventEntity.StreamId,
                Operation = eventEntity.Operation,
                Version = eventEntity.Version,
                OccurredAt = eventEntity.OccurredAt,
                Payload = new()
                {
                    {nameof(AccountChargedDto.ByCount), accountChargedDomainEvent.ByCount},
                    {nameof(AccountChargedDto.Reason), accountChargedDomainEvent.Reason ?? String.Empty}
                }
            };

            await _eventProjectionStore.CreateAsync(projection, cancellationToken);
        }
    }
}
