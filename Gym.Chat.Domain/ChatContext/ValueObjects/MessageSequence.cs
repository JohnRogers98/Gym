using Gym.Chat.Domain._Common;

namespace Gym.Domain.ChatContext.ValueObjects
{
    public record MessageSequence
    {
        public static readonly MessageSequence Zero = new(0);

        public Int32 Value { get; }

        private MessageSequence(Int32 value) => Value = value;

        public static Result<MessageSequence> From(Int32 value)
        {
            return Result<MessageSequence>.Ok(new(value));
        }
    }
}
