using Gym.Chat.Domain._Common;
using Gym.Domain.ChatContext.ValueObjects;
using Gym.Chat.Domain.MessageContext.ValueObjects;
using Gym.Chat.Domain.MessageContext.Events;
using Gym.Chat.Domain.SharedContext;

namespace Gym.Chat.Domain.MessageContext
{
    public class Message : AggregateRoot
    {
        public MessageId Id { get; }
        public ChatId ChatId { get; }
        public UserId AuthorId { get; }
        public MessageContent Content { get; }
        public MessageStatus Status { get; }
        public CreatedAt CreatedAt { get; }
        public MessageVersion Version { get; }
        public MessageSequence Sequence { get; }

        public MessageId? RepliedTo { get; }

        private Message(
            MessageId id,
            ChatId chatId,
            UserId authorId,
            MessageContent content,
            MessageStatus messageStatus,
            CreatedAt createdAt,
            MessageVersion version,
            MessageSequence sequence,
            MessageId? repliedToMessage)
        {
            Id = id;
            ChatId = chatId;
            AuthorId = authorId;
            Content = content;
            Status = messageStatus;
            CreatedAt = createdAt;
            Version = version;
            Sequence = sequence;
            RepliedTo = repliedToMessage;
        }

        public static Message Create(
            MessageId id,
            ChatId chatId,
            UserId authorId,
            MessageContent content,
            CreatedAt createdAt,
            MessageVersion version,
            MessageSequence sequence,
            MessageId? repliedToMessage = null)
        {
            Message message = new(id, chatId, authorId, content, MessageStatus.Sent, createdAt, version, sequence, repliedToMessage);
            message.AddDomainEvent(MessageCreatedDomainEvent.Create(message.Id));
            return message;
        }

        public static Message Restore(
            MessageId id,
            ChatId chatId,
            UserId authorId,
            MessageContent content,
            MessageStatus messageStatus,
            CreatedAt createdAt,
            MessageVersion version,
            MessageSequence sequence,
            MessageId? repliedToMessage = null)
            => new(id, chatId, authorId, content, messageStatus, createdAt, version, sequence, repliedToMessage);



        public override String ToString()
            => $"{nameof(Id)}: {Id} \t {nameof(Content)}: {Content} \t {nameof(CreatedAt)}: {CreatedAt}";
        
        public override Boolean Equals(Object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Message other && Id == other.Id;
        }
        public override Int32 GetHashCode() => Id.GetHashCode();
    }
}
