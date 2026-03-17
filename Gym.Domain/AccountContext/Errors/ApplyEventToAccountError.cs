using Gym.Domain._Common;
using Gym.Domain.AccountContext.ValueObjects;

namespace Gym.Domain.AccountContext.Errors
{
    public class ApplyEventToAccountError : DomainError
    {
        public AccountId AccountId { get; }

        public DomainEvent Event { get; }

        private ApplyEventToAccountError(AccountId accountId, DomainEvent @event) : base(nameof(ApplyEventToAccountError))
        {
            AccountId = accountId;
            Event = @event;
        }

        public static ApplyEventToAccountError Create(AccountId accountId, DomainEvent @event) => new(accountId, @event);

        public override String GetErrorMessage() => $"Cannot apply event - {Event.Id} to account - {AccountId}";
    }
}
