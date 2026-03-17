using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext.Errors;

namespace Gym.Domain.AccountContext.ValueObjects
{
    public record AccountId
    {
        public String Value { get; }

        private AccountId(String value) => Value = value;

        public static Result<AccountId> From(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Result<AccountId>.Fail(AccountIdValidationError.Create());
            }

            return Result<AccountId>.Ok(new(value));
        }

        public static AccountId From(UserId userId) => new ($"account_{userId.Value}");

        public override String ToString() => Value.ToString();
    }
}
