using Gym.Chat.Domain._Common;
using Gym.Chat.Domain.MessageContext.ValueObjects;

namespace Gym.Chat.Domain.MessageContext.Events
{
    public class MessageCreatedDomainEvent : DomainEvent
    {
        public MessageId MessageId { get; private set; }

        private MessageCreatedDomainEvent(DomainEventId id, DateTime occurredOn, MessageId messageId)
            : base(id, occurredOn)
            => (MessageId) = (messageId);

        public static MessageCreatedDomainEvent Create(MessageId messageId)
            => new(DomainEventId.Generate(), DateTime.UtcNow, messageId);

        public static MessageCreatedDomainEvent Restore(DomainEventId id, DateTime occurredOn, MessageId messageId)
           => new(id, occurredOn, messageId);
    }
}
