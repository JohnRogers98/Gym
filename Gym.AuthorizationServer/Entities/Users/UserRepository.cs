using MongoDB.Driver;

namespace Gym.AuthorizationServer.Entities.Users
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetByIdAsync(String id, CancellationToken cancellationToken);
        Task<IEnumerable<UserEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(UserEntity entity, CancellationToken cancellationToken);
        Task<Boolean> ExistsAsync(String id, CancellationToken cancellationToken);
    }

    public class UserRepository(IMongoCollection<UserEntity> _users) : IUserRepository
    {
        public async Task AddAsync(UserEntity entity, CancellationToken cancellationToken)
        {
            await _users.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }

        public async Task<Boolean> ExistsAsync(String id, CancellationToken cancellationToken)
        {
            return await _users.Find(eUser => eUser.Id == id)
                .AnyAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _users.Find(Builders<UserEntity>.Filter.Empty)
                .ToListAsync(cancellationToken);
        }

        public async Task<UserEntity?> GetByIdAsync(String id, CancellationToken cancellationToken)
        {
            return await _users.Find(eUser => eUser.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
