using Gym.Chat.Domain._Common;
using Gym.Chat.Domain.MessageContext.Errors;

namespace Gym.Chat.Domain.MessageContext.ValueObjects
{
    public class MessageId
    {
        public String Value { get; }

        private MessageId(String value) => Value = value;

        public static Result<MessageId> From(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Result<MessageId>.Fail(MessageIdValidationError.Create());
            }

            return Result<MessageId>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
