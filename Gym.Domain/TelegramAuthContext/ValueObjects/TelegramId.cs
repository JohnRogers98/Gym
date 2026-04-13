using Gym.Domain._Common;
using Gym.Domain.TelegramAuthContext.Errors;

namespace Gym.Domain.TelegramAuthContext.ValueObjects
{
    public record TelegramId
    {
        public Int64 Value { get; }

        private TelegramId(Int64 value) => Value = value;

        public static Result<TelegramId> From(Int64 value)
        {
            if (value < 0)
            {
                return Result<TelegramId>.Fail(TelegramIdValidationError.Create());
            }

            return Result<TelegramId>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
