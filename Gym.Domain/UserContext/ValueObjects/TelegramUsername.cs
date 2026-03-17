using Gym.Domain._Common;
using Gym.Domain.UserContext.Errors;

namespace Gym.Domain.UserContext.ValueObjects
{
    public record TelegramUsername
    {
        public String Value { get; }

        private TelegramUsername(String value) => Value = value;

        public static Result<TelegramUsername> From(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Result<TelegramUsername>.Fail(TelegramUsernameValidationError.Create());
            }

            return Result<TelegramUsername>.Ok(new(value)); 
        }

        public override String ToString() => Value.ToString();
    }
}
