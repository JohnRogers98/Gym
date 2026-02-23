using Gym.Domain._Common;
using Gym.Domain._Shared;

namespace Gym.Domain.ClientContext.Events
{
    public class ClientCreatedDomainEvent : DomainEvent
    {
        public UserId UserId { get; private set; }

        private ClientCreatedDomainEvent(DomainEventId id, DateTime occurredOn, UserId userId)
            : base(id, occurredOn) 
            => (UserId) = (userId);

        public static ClientCreatedDomainEvent Create(UserId userId)
            => new(DomainEventId.Generate(), DateTime.Now, userId);

        public static ClientCreatedDomainEvent Restore(DomainEventId id, DateTime occurredOn, UserId userId)
           => new(id, occurredOn, userId);
    }
}
