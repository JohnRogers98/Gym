using Gym.Domain._Common;
using Gym.Domain._Shared;

namespace Gym.Domain.AccountContext.Events
{
    public class AccountChargedDomainEvent : DomainEvent
    {
        public UserId UserId { get; private set; }
        
        public Int32 ByCount { get; private set;  }
        
        public String? Reason { get; private set; }

        private AccountChargedDomainEvent(DomainEventId id, DateTime occurredOn, UserId userId, Int32 byCount, String? reason) 
            : base(id, occurredOn)
            => (UserId, ByCount, Reason) = (userId, byCount, reason);

        public static AccountChargedDomainEvent Create(UserId userId, Int32 byCount, String? reason = default) 
            => new(DomainEventId.Generate(), DateTime.Now, userId, byCount, reason);

        public static AccountChargedDomainEvent Restore(DomainEventId id, DateTime occurredOn, UserId userId, Int32 byCount, String? reason = default)
            => new(id, occurredOn, userId, byCount, reason);
    }
}
