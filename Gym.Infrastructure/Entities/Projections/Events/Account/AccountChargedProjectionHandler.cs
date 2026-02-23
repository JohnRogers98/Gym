using Gym.Abstractions.Query.EventStore;
using Gym.Domain.AccountContext.Events;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.DtoDeserializers;
using Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto;

namespace Gym.Infrastructure.Entities.Projections.Events.Account
{
    internal class AccountChargedProjectionHandler(IEventDtoDeserializer _eventDtoDeserializer, EventProjectionStore _eventProjectionStore) : IProjectionHandler
    {
        public Boolean CanHandle(String aggregateType, String operation)
        {
            return aggregateType == nameof(Domain.AccountContext.Account) && operation == nameof(AccountChargedDomainEvent);
        }

        public async Task HandleAsync(EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var accountChargedDto = _eventDtoDeserializer.Deserialize<AccountChargedDto>(eventEntity);

            var projection = new EventProjection()
            {
                Id = eventEntity.Id,
                StreamId = eventEntity.StreamId,
                Operation = eventEntity.Operation,
                Version = eventEntity.Version,
                OccurredAt = eventEntity.OccurredAt,
                Payload = new()
                {
                    {nameof(AccountChargedDto.ByCount), accountChargedDto.ByCount},
                    {nameof(AccountChargedDto.Reason), accountChargedDto.Reason ?? String.Empty}
                }
            };

            await _eventProjectionStore.CreateAsync(projection, cancellationToken);
        }
    }
}
