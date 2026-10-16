using Gym.Chat.Domain.MessageContext.ValueObjects;

namespace Gym.Chat.Domain.MessageContext
{
    public interface IMessageRepository
    {
        MessageId NextIdentity();
        Task SaveAsync(Message message, CancellationToken cancellationToken);
        Task<Message?> GetByIdAsync(MessageId id, CancellationToken cancellationToken);
    }
}
