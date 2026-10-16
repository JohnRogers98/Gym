using Gym.Chat.Domain._Common;
using Gym.Chat.Domain.MessageContext.Errors;

namespace Gym.Chat.Domain.MessageContext.ValueObjects
{
    public record MessageContent
    {
        public String? Text { get; init; }
        public Attachment[]? Attachments { get; init; }

        public static Result<MessageContent> From(String? text, Attachment[]? attachments)
        {
            if (String.IsNullOrWhiteSpace(text) && (attachments == null || attachments.Length == 0))
            {
                return Result<MessageContent>.Fail(MessageContentValidationError.Create(text, attachments));
            }
            return Result<MessageContent>.Ok(new MessageContent { Text = text, Attachments = attachments });
        }
    }

    public record Attachment;
}
