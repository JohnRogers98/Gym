using Gym.Domain._Common;

namespace Gym.Domain.InstructorContext.Events
{
    public class InstructorCreatedDomainEvent : DomainEvent
    {
        public InstructorId InstructorId { get; private set; }

        private InstructorCreatedDomainEvent(DomainEventId id, DateTime occurredOn, InstructorId instructorId)
            : base(id, occurredOn)
            => (InstructorId) = (instructorId);

        public static InstructorCreatedDomainEvent Create(InstructorId instructorId)
            => new(DomainEventId.Generate(), DateTime.Now, instructorId);

        public static InstructorCreatedDomainEvent Restore(DomainEventId id, DateTime occurredOn, InstructorId instructorId)
           => new(id, occurredOn, instructorId);
    }
}
