using Gym.AuthorizationServer.Infrastructure.Session;
using MongoDB.Driver;

namespace Gym.AuthorizationServer.Infrastructure.Entities.Roles
{
    public interface IRoleRepository
    {
        Task<UserRoleEntity?> GetByIdAsync(String id, CancellationToken cancellationToken);
        Task AddAsync(UserRoleEntity entity, CancellationToken cancellationToken);
        Task<Boolean> ExistsByNameAsync(String roleName, CancellationToken cancellationToken);
        Task<UserRoleEntity?> GetByNameAsync(String name, CancellationToken cancellationToken);
    }

    internal class UserRoleRepository(IMongoCollection<UserRoleEntity> _roles, MongoUnitOfWork _mongoUnitOfWork) : IRoleRepository
    {
        public async Task AddAsync(UserRoleEntity entity, CancellationToken cancellationToken)
        {
            await _roles.InsertOneAsync(_mongoUnitOfWork.Session, entity, cancellationToken: cancellationToken);
        }

        public async Task<Boolean> ExistsByNameAsync(String roleName, CancellationToken cancellationToken)
        {
            return await _roles.Find(_mongoUnitOfWork.Session, eUser => eUser.Name == roleName)
                .AnyAsync(cancellationToken);
        }

        public async Task<UserRoleEntity?> GetByIdAsync(String id, CancellationToken cancellationToken)
        {
            return await _roles.Find(_mongoUnitOfWork.Session, eRole => eRole.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<UserRoleEntity?> GetByNameAsync(String name, CancellationToken cancellationToken)
        {
            return await _roles.Find(_mongoUnitOfWork.Session, eRole => eRole.Name == name)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
