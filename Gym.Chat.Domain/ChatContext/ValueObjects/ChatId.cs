using Gym.Chat.Domain._Common;
using Gym.Chat.Domain.MessageContext.Errors;

namespace Gym.Domain.ChatContext.ValueObjects
{
    public class ChatId
    {
        public String Value { get; }

        private ChatId(String value) => Value = value;

        public static Result<ChatId> From(String value)
        {
            if (!IsValid(value))
            {
                return Result<ChatId>.Fail(ChatIdValidationError.Create());
            }

            return Result<ChatId>.Ok(new(value));
        }

        private static Boolean IsValid(String value){
            return !String.IsNullOrWhiteSpace(value) 
                && (value.StartsWith("chat_") || value.StartsWith("group_"));
        }

        public override String ToString() => Value.ToString();
    }
}
  