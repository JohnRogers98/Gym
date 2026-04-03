using Gym.Application.Extensions;
using Gym.Domain._Shared;
using Gym.Domain.UserContext;
using Gym.Infrastructure.Entities.Extensions;
using Gym.Infrastructure.Entities.Extensions.Mappings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Repositories.Users
{
    internal class UserRepository(IMongoCollection<UserEntity> _userCollection, MongoUnitOfWork _mongoUnitOfWork) : IUserRepository
    {
        public UserId NextIdentity() => UserId.From(ObjectId.GenerateNewId().ToString()).Unwrap();

        public async Task SaveAsync(User user, CancellationToken cancellationToken)
        {
            UserEntity userEntity = user.ToEntity();

            await _userCollection.ReplaceOneAsync(
                _mongoUnitOfWork.Session,
                eUser => eUser.Id == userEntity.Id,
                userEntity,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
        }

        public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken)
        {
            var foundedEntity = await _userCollection.Find(_mongoUnitOfWork.Session, eUser => eUser.Id == id.Value.ToObjectId())
             .FirstOrDefaultAsync(cancellationToken);

            return foundedEntity?.ToDomain();
        }

        public async Task<Boolean> ExistsAsync(UserId id, CancellationToken cancellationToken)
            => await _userCollection.Find(_mongoUnitOfWork.Session, eUser => eUser.Id == id.Value.ToObjectId()).AnyAsync(cancellationToken);
    }
}
