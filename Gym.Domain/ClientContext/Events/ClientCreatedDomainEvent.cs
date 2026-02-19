using Gym.Domain._Common;
using Gym.Domain._Shared;

namespace Gym.Domain.ClientContext.Events
{
    public class ClientCreatedDomainEvent : DomainEvent
    {
        public UserId UserId { get; private set; }

        private ClientCreatedDomainEvent(DomainEventId id, DateTime occuredOn, UserId userId)
            : base(id, occuredOn) 
            => (UserId) = (userId);

        public static ClientCreatedDomainEvent Create(UserId userId)
            => new(DomainEventId.Generate(), DateTime.Now, userId);

        public static ClientCreatedDomainEvent Restore(DomainEventId id, DateTime occuredOn, UserId userId)
           => new(id, occuredOn, userId);
    }
}
