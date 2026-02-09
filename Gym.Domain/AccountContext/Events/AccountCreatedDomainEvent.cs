using Gym.Domain._Common;

namespace Gym.Domain.AccountContext.Events
{
    public class AccountCreatedDomainEvent : DomainEvent
    {
        private AccountCreatedDomainEvent(DomainEventId id, DateTime occuredOn) { }

        public static AccountCreatedDomainEvent Create()
            => new(DomainEventId.Generate(), DateTime.Now);

        public static AccountCreatedDomainEvent Restore(DomainEventId id, DateTime occuredOn)
            => new(id, occuredOn);
    }
}
