using Gym.Domain._Common;
using Gym.Domain.PersonalTrainingContext;
using Gym.Domain.PersonalTrainingContext.ValueObjects;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.Serializers;

namespace Gym.Infrastructure.Entities.Repositories.PersonalTrainings
{
    internal class PersonalTrainingEventStoreAspect(
        IPersonalTrainingRepository _decoratee,
        IEventStore _eventStore,
        IEventSerializer _eventSerializer) : IPersonalTrainingRepository
    {
        public async Task<Boolean> ExistsAsync(PersonalTrainingId id, CancellationToken cancellationToken)
        {
            return await _decoratee.ExistsAsync(id, cancellationToken);
        }

        public async Task<PersonalTraining?> GetByIdAsync(PersonalTrainingId id, CancellationToken cancellationToken)
        {
            return await _decoratee.GetByIdAsync(id, cancellationToken);
        }

        public PersonalTrainingId NextIdentity()
        {
            return _decoratee.NextIdentity();
        }

        public async Task SaveAsync(PersonalTraining personalTraining, CancellationToken cancellationToken)
        {
            if (personalTraining.DomainEvents.Any())
            {
                await _eventStore.SaveAutoversionedAsync(
                    this.CreateStreamId(personalTraining.Id),
                    personalTraining.DomainEvents.Select(domainEvent => this.CreateEventEntity(personalTraining.Id, domainEvent)).ToList(),
                    cancellationToken
                    );
            }
            await _decoratee.SaveAsync(personalTraining, cancellationToken);

            personalTraining.ClearDomainEvents();
        }

        private EventEntity CreateEventEntity(PersonalTrainingId personalTrainingId, DomainEvent domainEvent)
        {
            return new EventEntity()
            {
                Id = domainEvent.Id.Value.ToString(),
                StreamId = this.CreateStreamId(personalTrainingId).Value,
                AggregateType = nameof(PersonalTraining),
                Operation = domainEvent.GetType().Name,
                Data = _eventSerializer.Serialize(domainEvent),
                OccurredAt = domainEvent.OccurredOn
            };
        }

        private StreamId CreateStreamId(PersonalTrainingId personalTrainingId)
            => new StreamId($"personalTraining_{personalTrainingId.Value}");

    }
}
