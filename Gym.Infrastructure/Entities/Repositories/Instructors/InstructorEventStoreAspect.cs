using Gym.Domain._Common;
using Gym.Domain.InstructorContext;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.Serializers;

namespace Gym.Infrastructure.Entities.Repositories.Instructors
{
    internal class InstructorEventStoreAspect(
        IInstructorRepository _decoratee,
        IEventStore _eventStore,
        IEventSerializer _eventSerializer) : IInstructorRepository
    {
        public async Task<Boolean> ExistsAsync(InstructorId id, CancellationToken cancellationToken)
        {
            return await _decoratee.ExistsAsync(id, cancellationToken);
        }

        public async Task<Instructor?> GetByIdAsync(InstructorId id, CancellationToken cancellationToken)
        {
            return await GetByIdAsync(id, cancellationToken);
        }

        public InstructorId NextIdentity()
        {
            return _decoratee.NextIdentity();
        }

        public async Task SaveAsync(Instructor instructor, CancellationToken cancellationToken)
        {
            if (instructor.DomainEvents.Any())
            {
                await _eventStore.SaveAutoversionedAsync(
                    this.CreateStreamId(instructor.Id),
                    instructor.DomainEvents.Select(domainEvent => this.CreateEventEntity(instructor.Id, domainEvent)).ToList(),
                    cancellationToken
                    );
            }
            await _decoratee.SaveAsync(instructor, cancellationToken);

            instructor.ClearDomainEvents();
        }

        private EventEntity CreateEventEntity(InstructorId instructorId, DomainEvent domainEvent)
        {
            return new EventEntity()
            {
                Id = domainEvent.Id.Value.ToString(),
                StreamId = this.CreateStreamId(instructorId).Value,
                AggregateType = nameof(Instructor),
                Operation = domainEvent.GetType().Name,
                Data = _eventSerializer.Serialize(domainEvent),
                OccurredAt = domainEvent.OccurredOn
            };
        }

        private StreamId CreateStreamId(InstructorId instructorId)
            => new StreamId($"instructor_{instructorId.Value}");
    }
}
