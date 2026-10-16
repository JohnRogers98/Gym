using Gym.Chat.Domain._Common;
using Gym.Domain.ChatContext.ValueObjects;

namespace Gym.Domain.ChatContext.Events
{
    public class DirectChatCreatedDomainEvent : DomainEvent
    {
        public ChatId ChatId { get; private set; }

        private DirectChatCreatedDomainEvent(DomainEventId id, DateTime occurredOn, ChatId chatId)
            : base(id, occurredOn)
            => (ChatId) = (chatId);

        public static DirectChatCreatedDomainEvent Create(ChatId chatId)
            => new(DomainEventId.Generate(), DateTime.UtcNow, chatId);

        public static DirectChatCreatedDomainEvent Restore(DomainEventId id, DateTime occurredOn, ChatId chatId)
           => new(id, occurredOn, chatId);
    }
}
