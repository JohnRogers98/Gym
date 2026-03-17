using Gym.Domain._Common;

namespace Gym.Domain.AccountContext.Errors
{
    public class AccountIdValidationError : DomainError
    {
        private AccountIdValidationError() : base(nameof(AccountIdValidationError)) { }

        public static AccountIdValidationError Create() => new();

        public override String GetErrorMessage() => $"Account id is invalid.";
    }
}
