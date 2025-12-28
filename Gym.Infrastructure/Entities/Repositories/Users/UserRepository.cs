using Gym.Domain.UserAggregate;
using Gym.Infrastructure.Entities.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Repositories.Users
{
    internal class UserRepository(IMongoCollection<UserEntity> _userCollection) : IUserRepository, IUserQueryService
    {
        public UserId NextIdentity() => UserId.From(ObjectId.GenerateNewId().ToString());

        public async Task<User?> GetByTelegramIdAsync(TelegramId telegramUserId, CancellationToken cancellationToken)
        {
            var foundedEntity = await _userCollection.Find(eUser => eUser.TelegramId == telegramUserId.Value)
               .FirstOrDefaultAsync(cancellationToken);

            return foundedEntity?.ToDomain();
        }

        public async Task<Boolean> ExistsByTelegramIdAsync(TelegramId telegramUserId, CancellationToken cancellationToken) 
            => await _userCollection.Find(eUser => eUser.TelegramId == telegramUserId.Value).AnyAsync(cancellationToken);


        public async Task SaveAsync(User user, CancellationToken cancellationToken)
        {
            UserEntity userEntity = user.ToEntity();

            await _userCollection.ReplaceOneAsync(
                eUser => eUser.Id == userEntity.Id,
                userEntity,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
        }

        public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken)
        {
            var foundedEntity = await _userCollection.Find(eUser => eUser.Id == id.Value.ToObjectId())
             .FirstOrDefaultAsync(cancellationToken);

            return foundedEntity?.ToDomain();
        }

        public async Task<Boolean> ExistsAsync(UserId id, CancellationToken cancellationToken)
            => await _userCollection.Find(eUser => eUser.Id == id.Value.ToObjectId()).AnyAsync(cancellationToken);
    }
}
