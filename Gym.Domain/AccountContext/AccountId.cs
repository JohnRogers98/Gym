using Gym.Domain._Shared;

namespace Gym.Domain.AccountContext
{
    public record AccountId
    {
        public String Value { get; }

        private AccountId(String value) => Value = value;

        public static AccountId From(String value) => new (value);

        public static AccountId From(UserId userId) => new ($"account_{userId.Value}");
    }
}
