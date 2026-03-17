using Gym.Domain._Common;
using Gym.Domain.TrainingContext;
using Gym.Domain.TrainingContext.ValueObjects;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.Serializers;

namespace Gym.Infrastructure.Entities.Repositories.Trainings
{
    internal class TrainingEventStoreAspect(
        ITrainingRepository _decoratee,
        IEventStore _eventStore,
        IEventSerializer _eventSerializer) : ITrainingRepository
    {
        public async Task<Boolean> ExistsAsync(TrainingId id, CancellationToken cancellationToken)
        {
            return await _decoratee.ExistsAsync(id, cancellationToken);
        }

        public async Task<Training?> GetByIdAsync(TrainingId id, CancellationToken cancellationToken)
        {
            return await _decoratee.GetByIdAsync(id, cancellationToken);
        }

        public TrainingId NextIdentity()
        {
            return _decoratee.NextIdentity();
        }

        public async Task SaveAsync(Training training, CancellationToken cancellationToken)
        {
            if (training.DomainEvents.Any())
            {
                await _eventStore.SaveAutoversionedAsync(
                    this.CreateStreamId(training.Id),
                    training.DomainEvents.Select(domainEvent => this.CreateEventEntity(training.Id, domainEvent)).ToList(),
                    cancellationToken
                    );
            }
            await _decoratee.SaveAsync(training, cancellationToken);

            training.ClearDomainEvents();
        }

        private EventEntity CreateEventEntity(TrainingId trainingId, DomainEvent domainEvent)
        {
            return new EventEntity()
            {
                Id = domainEvent.Id.Value.ToString(),
                StreamId = this.CreateStreamId(trainingId).Value,
                AggregateType = nameof(Training),
                Operation = domainEvent.GetType().Name,
                Data = _eventSerializer.Serialize(domainEvent),
                OccurredAt = domainEvent.OccurredOn
            };
        }

        private StreamId CreateStreamId(TrainingId trainingId)
            => new StreamId($"training_{trainingId.Value}");

    }
}
