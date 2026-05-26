using MongoDB.Driver;

namespace Gym.AuthorizationServer.Entities.Users.TelegramCredentials
{
    public interface ITelegramCredentialRepository
    {
        Task<TelegramCredentialEntity?> GetByIdAsync(Int64 id, CancellationToken cancellationToken);
        Task AddAsync(TelegramCredentialEntity entity, CancellationToken cancellationToken);
        Task<Boolean> ExistsAsync(Int64 id, CancellationToken cancellationToken);
    }

    public class TelegramCredentialRepository(IMongoCollection<TelegramCredentialEntity> _telegramCredentials) : ITelegramCredentialRepository
    {
        public async Task AddAsync(TelegramCredentialEntity entity, CancellationToken cancellationToken)
        {
            await _telegramCredentials.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }

        public async Task<Boolean> ExistsAsync(Int64 id, CancellationToken cancellationToken)
        {
            return await _telegramCredentials.Find(eTelgramCredential => eTelgramCredential.Id == id)
              .AnyAsync(cancellationToken);
        }

        public async Task<TelegramCredentialEntity?> GetByIdAsync(Int64 id, CancellationToken cancellationToken)
        {
            return await _telegramCredentials.Find(eTelgramCredential => eTelgramCredential.Id == id)
               .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
