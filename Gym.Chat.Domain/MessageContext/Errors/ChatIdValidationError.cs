using Gym.Chat.Domain._Common;

namespace Gym.Chat.Domain.MessageContext.Errors
{
    public class ChatIdValidationError : DomainError
    {
        private ChatIdValidationError() : base(nameof(ChatIdValidationError)) { }

        public static ChatIdValidationError Create() => new();

        public override String GetErrorMessage() => $"Chat id is invalid.";
    }
}
