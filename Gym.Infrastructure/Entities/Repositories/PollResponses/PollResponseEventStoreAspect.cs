using Gym.Domain._Common;
using Gym.Domain.PollResponseContext;
using Gym.Domain.PollResponseContext.ValueObjects;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.Serializers;

namespace Gym.Infrastructure.Entities.Repositories.Polls
{
    internal class PollResponseEventStoreAspect(
        IPollResponseRepository _decoratee,
        IEventStore _eventStore,
        IEventSerializer _eventSerializer) : IPollResponseRepository
    {
        public async Task<Boolean> ExistsAsync(PollResponseId id, CancellationToken cancellationToken)
        {
            return await _decoratee.ExistsAsync(id, cancellationToken);
        }

        public async Task<PollResponse?> GetByIdAsync(PollResponseId id, CancellationToken cancellationToken)
        {
            return await _decoratee.GetByIdAsync(id, cancellationToken);
        }

        public async Task SaveAsync(PollResponse pollResponse, CancellationToken cancellationToken)
        {
            if (pollResponse.DomainEvents.Any())
            {
                await _eventStore.SaveAutoversionedAsync(
                    this.CreateStreamId(pollResponse.Id),
                    pollResponse.DomainEvents.Select(domainEvent => this.CreateEventEntity(pollResponse.Id, domainEvent)).ToList(),
                    cancellationToken
                    );
            }
            await _decoratee.SaveAsync(pollResponse, cancellationToken);

            pollResponse.ClearDomainEvents();
        }

        private EventEntity CreateEventEntity(PollResponseId id, DomainEvent domainEvent)
        {
            return new EventEntity()
            {
                Id = domainEvent.Id.Value.ToString(),
                StreamId = this.CreateStreamId(id).Value,
                AggregateType = nameof(PollResponse),
                Operation = domainEvent.GetType().Name,
                Data = _eventSerializer.Serialize(domainEvent),
                OccurredAt = domainEvent.OccurredOn
            };
        }

        private StreamId CreateStreamId(PollResponseId pollResponseId)
            => new StreamId($"pollResponse_{pollResponseId.Value}");

    }
}
