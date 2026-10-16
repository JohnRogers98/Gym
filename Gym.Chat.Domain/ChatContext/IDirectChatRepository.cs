using Gym.Domain.ChatContext.ValueObjects;

namespace Gym.Domain.ChatContext
{
    public interface IDirectChatRepository
    {
        ChatId NextIdentity();
        Task SaveAsync(DirectChat chat, CancellationToken cancellationToken);
        Task<DirectChat?> GetByIdAsync(ChatId id, CancellationToken cancellationToken);
    }
}
