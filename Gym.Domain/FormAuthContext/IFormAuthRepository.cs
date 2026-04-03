using Gym.Domain.FormAuthContext.ValueObjects;

namespace Gym.Domain.FormAuthContext
{
    public interface IFormAuthRepository
    {
        Task SaveAsync(FormAuth formAuth, CancellationToken cancellationToken);
        Task<FormAuth?> GetByLoginAsync(Login login, CancellationToken cancellationToken);
        Task<Boolean> ExistsAsync(Login login, CancellationToken cancellationToken);
    }
}
