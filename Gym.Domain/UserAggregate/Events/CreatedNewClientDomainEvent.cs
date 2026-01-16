using Gym.Domain._Shared;

namespace Gym.Domain.UserAggregate.Events
{
    public class CreatedNewClientDomainEvent : DomainEvent
    {
        public UserId UserId { get; set; }

        private CreatedNewClientDomainEvent(UserId userId) => (UserId) = (userId);

        public static CreatedNewClientDomainEvent Create(UserId userId) => new(userId);
    }
}
