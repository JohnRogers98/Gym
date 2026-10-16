using Gym.Chat.Domain._Common;

namespace Gym.Chat.Domain.MessageContext.Errors
{
    public class MessageIdValidationError : DomainError
    {
        private MessageIdValidationError() : base(nameof(MessageIdValidationError)) { }

        public static MessageIdValidationError Create() => new();

        public override String GetErrorMessage() => $"Message id is invalid.";
    }
}
