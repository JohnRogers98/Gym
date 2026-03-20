using Gym.Domain._Common;

namespace Gym.Domain.AccountContext.Events
{
    public class AccountCreatedDomainEvent : DomainEvent
    {
        private AccountCreatedDomainEvent(DomainEventId id, DateTime occurredOn) { }

        public static AccountCreatedDomainEvent Create()
            => new(DomainEventId.Generate(), DateTime.UtcNow);

        public static AccountCreatedDomainEvent Restore(DomainEventId id, DateTime occurredOn)
            => new(id, occurredOn);
    }
}
