using Gym.Domain.PollResponseContext.ValueObjects;

namespace Gym.Domain.PollResponseContext
{
    public interface IPollResponseRepository
    {
        Task SaveAsync(PollResponse pollResponse, CancellationToken cancellationToken);
        Task<PollResponse?> GetByIdAsync(PollResponseId id, CancellationToken cancellationToken);
        Task<Boolean> ExistsAsync(PollResponseId id, CancellationToken cancellationToken);
    }
}
