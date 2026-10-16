using Gym.Chat.Domain._Common;

namespace Gym.Chat.Domain.MessageContext.ValueObjects
{
    public record CreatedAt
    {
        public DateTime Value { get; }

        private CreatedAt(DateTime value) => Value = value;

        public static Result<CreatedAt> From(DateTime value)
        {
            return Result<CreatedAt>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
