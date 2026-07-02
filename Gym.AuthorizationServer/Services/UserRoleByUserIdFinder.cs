using Gym.AuthorizationServer.Infrastructure.Entities.Roles;
using Gym.AuthorizationServer.Infrastructure.Entities.Users;
using Gym.AuthorizationServer.Shared.Abstractions;

namespace Gym.AuthorizationServer.Services
{
    public interface IUserRoleByUserIdFinder
    {
        Task<Result<UserRoleEntity>> FindAsync(String userId, CancellationToken cancellationToken);
    }

    public class UserRoleByUserIdFinder(IUserRepository _userRepository, IRoleRepository _roleRepository) : IUserRoleByUserIdFinder
    {
        public async Task<Result<UserRoleEntity>> FindAsync(String userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
                return Result<UserRoleEntity>.Failure("No user found.");

            var role = await _roleRepository.GetByIdAsync(user.RoleId, cancellationToken);
            return Result<UserRoleEntity>.Success(role!);
        }
    }
}
