using Gym.AuthorizationServer.Infrastructure.Session;
using MongoDB.Driver;

namespace Gym.AuthorizationServer.Infrastructure.Entities.Users.TelegramCredentials
{
    public interface ITelegramCredentialRepository
    {
        Task<TelegramCredentialEntity?> GetByIdAsync(Int64 id, CancellationToken cancellationToken);
        Task AddAsync(TelegramCredentialEntity entity, CancellationToken cancellationToken);
        Task<Boolean> ExistsAsync(Int64 id, CancellationToken cancellationToken);
    }

    public class TelegramCredentialRepository(IMongoCollection<TelegramCredentialEntity> _telegramCredentials, MongoUnitOfWork _mongoUnitOfWork) : ITelegramCredentialRepository
    {
        public async Task AddAsync(TelegramCredentialEntity entity, CancellationToken cancellationToken)
        {
            await _telegramCredentials.InsertOneAsync(_mongoUnitOfWork.Session, entity, cancellationToken: cancellationToken);
        }

        public async Task<Boolean> ExistsAsync(Int64 id, CancellationToken cancellationToken)
        {
            return await _telegramCredentials.Find(_mongoUnitOfWork.Session, eTelgramCredential => eTelgramCredential.Id == id)
              .AnyAsync(cancellationToken);
        }

        public async Task<TelegramCredentialEntity?> GetByIdAsync(Int64 id, CancellationToken cancellationToken)
        {
            return await _telegramCredentials.Find(_mongoUnitOfWork.Session, eTelgramCredential => eTelgramCredential.Id == id)
               .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
