using Gym.Domain._Common;
using Gym.Domain.ClientContext;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.Serializers;

namespace Gym.Infrastructure.Entities.Repositories.Clients
{
    internal class ClientEventStoreAspect(IClientRepository _decoratee, IEventStore _eventStore, IEventSerializer _eventSerializer) : IClientRepository
    {
        public async Task<Boolean> ExistsAsync(ClientId id, CancellationToken cancellationToken)
        {
            return await _decoratee.ExistsAsync(id, cancellationToken);
        }

        public async Task<Client?> GetByIdAsync(ClientId id, CancellationToken cancellationToken)
        {
            return await _decoratee.GetByIdAsync(id, cancellationToken);
        }

        public ClientId NextIdentity()
        {
            return _decoratee.NextIdentity();
        }

        public async Task SaveAsync(Client client, CancellationToken cancellationToken)
        {
            if (client.DomainEvents.Any())
            {
                await _eventStore.SaveAutoversionedAsync(
                    this.CreateStreamId(client.Id),
                    client.DomainEvents.Select(domainEvent => this.CreateEventEntity(client.Id, domainEvent)).ToList(),
                    cancellationToken
                    );
            }
            await _decoratee.SaveAsync(client, cancellationToken);

            client.ClearDomainEvents();
        }

        private EventEntity CreateEventEntity(ClientId clientId, DomainEvent domainEvent)
        {
            return new EventEntity()
            {
                Id = domainEvent.Id.Value.ToString(),
                StreamId = this.CreateStreamId(clientId).Value,
                AggregateType = nameof(Client),
                Operation = domainEvent.GetType().Name,
                Data = _eventSerializer.Serialize(domainEvent),
                OccurredAt = domainEvent.OccurredOn
            };
        }

        private StreamId CreateStreamId(ClientId clientId)
            => new StreamId($"client_{clientId.Value}");
    }
}
