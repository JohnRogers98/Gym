using Gym.Chat.Domain.SharedContext;

namespace Gym.Domain.ChatContext
{
    public interface IDirectChatByPeerIdsFinder
    {
        Task<DirectChat?> GetByPeerIdsAsync(UserId userId, CancellationToken cancellationToken);
    }
}
