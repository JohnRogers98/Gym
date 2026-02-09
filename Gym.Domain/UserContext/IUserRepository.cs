using Gym.Domain._Shared;

namespace Gym.Domain.UserContext
{
    public interface IUserRepository
    {
        UserId NextIdentity();
        Task SaveAsync(User user, CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken);
        Task<Boolean> ExistsAsync(UserId id, CancellationToken cancellationToken);
    }
}
