using Gym.Chat.Domain._Common;
using Gym.Chat.Domain.MessageContext.ValueObjects;

namespace Gym.Chat.Domain.MessageContext.Errors
{
    public class MessageContentValidationError : DomainError
    {
        public MessageContentValidationError() : base(nameof(MessageContentValidationError)) { }

        public static MessageContentValidationError Create(String? text, Attachment[]? attachments) => new();

        public override String GetErrorMessage() => $"Message content is invalid.";
    }
}