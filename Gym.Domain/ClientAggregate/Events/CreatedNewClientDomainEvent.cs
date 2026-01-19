using Gym.Domain._Common;
using Gym.Domain._Shared;

namespace Gym.Domain.ClientAggregate.Events
{
    public class CreatedNewClientDomainEvent : DomainEvent
    {
        public UserId UserId { get; private set; }

        private CreatedNewClientDomainEvent(UserId userId) => (UserId) = (userId);

        public static CreatedNewClientDomainEvent Create(UserId userId) => new(userId);
    }
}
