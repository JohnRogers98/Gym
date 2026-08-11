using Gym.Domain._Common;
using Gym.Domain.ClientContext.ValueObjects;
using Gym.Domain.InstructorContext.ValueObjects;
using Gym.Domain.PersonalTrainingContext.ValueObjects;

namespace Gym.Domain.PersonalTrainingContext.Events
{
    public class PersonalTrainingCancelledDomainEvent : DomainEvent
    {
        public PersonalTrainingId PersonalTrainingId { get; private set; }
        public InstructorId InstructorId { get; private set; }
        public ClientId ClientId { get; private set; }


        private PersonalTrainingCancelledDomainEvent(DomainEventId id, DateTime occurredOn, PersonalTrainingId personalTrainingId, InstructorId instructorId, ClientId clientId)
            : base(id, occurredOn)
            => (PersonalTrainingId, InstructorId, ClientId) = (personalTrainingId, instructorId, clientId);

        public static PersonalTrainingCancelledDomainEvent Create(PersonalTrainingId personalTrainingId, InstructorId instructorId, ClientId clientId)
            => new(DomainEventId.Generate(), DateTime.UtcNow, personalTrainingId, instructorId, clientId);

        public static PersonalTrainingCancelledDomainEvent Restore(DomainEventId id, DateTime occurredOn, PersonalTrainingId personalTrainingId, InstructorId instructorId, ClientId clientId)
           => new(id, occurredOn, personalTrainingId, instructorId, clientId);
    }
}
