using Gym.Domain._Common;
using Gym.Domain.PollContext;
using Gym.Domain.PollContext.ValueObjects;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.Serializers;

namespace Gym.Infrastructure.Entities.Repositories.Polls
{
    internal class PollEventStoreAspect(
        IPollRepository _decoratee,
        IEventStore _eventStore,
        IEventSerializer _eventSerializer) : IPollRepository
    {
        public async Task<Boolean> ExistsAsync(PollId id, CancellationToken cancellationToken)
        {
            return await _decoratee.ExistsAsync(id, cancellationToken);
        }

        public async Task<Poll?> GetByIdAsync(PollId id, CancellationToken cancellationToken)
        {
            return await _decoratee.GetByIdAsync(id, cancellationToken);
        }

        public PollId NextIdentity()
        {
            return _decoratee.NextIdentity();
        }

        public async Task SaveAsync(Poll poll, CancellationToken cancellationToken)
        {
            if (poll.DomainEvents.Any())
            {
                await _eventStore.SaveAutoversionedAsync(
                    this.CreateStreamId(poll.Id),
                    poll.DomainEvents.Select(domainEvent => this.CreateEventEntity(poll.Id, domainEvent)).ToList(),
                    cancellationToken
                    );
            }
            await _decoratee.SaveAsync(poll, cancellationToken);

            poll.ClearDomainEvents();
        }

        private EventEntity CreateEventEntity(PollId pollId, DomainEvent domainEvent)
        {
            return new EventEntity()
            {
                Id = domainEvent.Id.Value.ToString(),
                StreamId = this.CreateStreamId(pollId).Value,
                AggregateType = nameof(Poll),
                Operation = domainEvent.GetType().Name,
                Data = _eventSerializer.Serialize(domainEvent),
                OccurredAt = domainEvent.OccurredOn
            };
        }

        private StreamId CreateStreamId(PollId pollId)
            => new StreamId($"poll_{pollId.Value}");

    }
}
