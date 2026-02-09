using Gym.Domain._Common;
using Gym.Domain._Shared;

namespace Gym.Domain.AccountContext.Errors
{
    public class AccountNotChargedError : DomainError
    {
        public UserId UserId { get; }

        private AccountNotChargedError(UserId userId) : base(nameof(AccountNotChargedError))
        {
            UserId = userId;
        }

        public static AccountNotChargedError Create(UserId userId) => new(userId);

        public override String GetErrorMessage() => $"Account of user - {UserId} is not charged for booking.";
    }
}
