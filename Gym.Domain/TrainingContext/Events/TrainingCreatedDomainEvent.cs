using Gym.Domain._Common;
using Gym.Domain.TrainingContext.ValueObjects;

namespace Gym.Domain.TrainingContext.Events
{
    public class TrainingCreatedDomainEvent : DomainEvent
    {
        public TrainingId TrainingId { get; private set; }

        private TrainingCreatedDomainEvent(DomainEventId id, DateTime occurredOn, TrainingId trainingId)
            : base(id, occurredOn)
            => (TrainingId) = (trainingId);

        public static TrainingCreatedDomainEvent Create(TrainingId trainingId)
            => new(DomainEventId.Generate(), DateTime.UtcNow, trainingId);

        public static TrainingCreatedDomainEvent Restore(DomainEventId id, DateTime occurredOn, TrainingId trainingId)
           => new(id, occurredOn, trainingId);
    }
}
