using Gym.Domain._Shared;
using Gym.Domain.TelegramAuthContext;
using Gym.Domain.TelegramAuthContext.ValueObjects;
using Gym.Infrastructure.Entities.Extensions;
using Gym.Infrastructure.Entities.Extensions.Mappings;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Repositories.TelgramAuths
{
    internal class TelegramAuthRepository(IMongoCollection<TelegramAuthEntity> _telegramAuthCollection, MongoUnitOfWork _mongoUnitOfWork) 
        : ITelegramAuthRepository, ITelegramAuthByUserIdFinder
    {   
        public async Task SaveAsync(TelegramAuth telegramAuth, CancellationToken cancellationToken)
        {
            TelegramAuthEntity telegramAuthEntity = telegramAuth.ToEntity();

            await _telegramAuthCollection.ReplaceOneAsync(
                _mongoUnitOfWork.Session,
                eTelegramAuth => eTelegramAuth.Id == telegramAuthEntity.Id,
                telegramAuthEntity,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
        }

        public async Task<TelegramAuth?> GetByIdAsync(TelegramId id, CancellationToken cancellationToken)
        {
            var foundedEntity = await _telegramAuthCollection.Find(_mongoUnitOfWork.Session, eTelegramAuth => eTelegramAuth.Id == id.Value)
                .FirstOrDefaultAsync(cancellationToken);

            return foundedEntity?.ToDomain();
        }

        public async Task<Boolean> ExistsAsync(TelegramId id, CancellationToken cancellationToken)
            => await _telegramAuthCollection.Find(_mongoUnitOfWork.Session, eTelegramAuth => eTelegramAuth.Id == id.Value).AnyAsync(cancellationToken);

        public async Task<TelegramAuth?> GetTelegramAuthByUserIdAsync(UserId userId, CancellationToken cancellationToken)
        {
            var foundedEntity = await _telegramAuthCollection.Find(_mongoUnitOfWork.Session, eTelegramAuth => eTelegramAuth.UserId == userId.Value.ToObjectId())
              .FirstOrDefaultAsync(cancellationToken);

            return foundedEntity?.ToDomain();
        }
    }
}
