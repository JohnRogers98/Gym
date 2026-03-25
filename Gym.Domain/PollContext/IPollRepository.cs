using Gym.Domain.PollContext.ValueObjects;

namespace Gym.Domain.PollContext
{
    public interface IPollRepository
    {
        PollId NextIdentity();
        Task SaveAsync(Poll poll, CancellationToken cancellationToken);
        Task<Poll?> GetByIdAsync(PollId id, CancellationToken cancellationToken);
        Task<Boolean> ExistsAsync(PollId id, CancellationToken cancellationToken);
    }
}
