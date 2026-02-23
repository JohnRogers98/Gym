using Gym.Domain._Common;
using Gym.Domain._Shared;

namespace Gym.Domain.UserContext.Events
{
    public class UserCreatedDomainEvent : DomainEvent
    {
        public UserId UserId { get; private set; }

        private UserCreatedDomainEvent(DomainEventId id, DateTime occurredOn, UserId userId) 
            : base(id, occurredOn) 
            => (UserId) = userId; 

        public static UserCreatedDomainEvent Create(UserId userId)
            => new (DomainEventId.Generate(), DateTime.Now, userId);

        public static UserCreatedDomainEvent Restore(DomainEventId id, DateTime occurredOn, UserId userId)
            => new(id, occurredOn, userId);
    }
}
