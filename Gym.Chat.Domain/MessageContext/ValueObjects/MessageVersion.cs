using Gym.Chat.Domain._Common;

namespace Gym.Chat.Domain.MessageContext.ValueObjects
{
    public record MessageVersion
    {
        public Int32 Value { get; }

        private MessageVersion(Int32 value) => Value = value;

        public static Result<MessageVersion> From(Int32 value)
        {
            return Result<MessageVersion>.Ok(new(value));
        }
    }
}
