using MongoDB.Driver;

namespace Gym.AuthorizationServer.Entities.UserConsents
{
    public interface IUserConsentRepository
    {
        Task<UserConsentEntity?> GetByUserIdAndClientIdAsync(String userId, String clientId, CancellationToken cancellationToken);
        Task AddAsync(UserConsentEntity entity, CancellationToken cancellationToken);
        Task UpdateGrantedScopesAsync(String userId, String clientId, List<String> grantedScopes, DateTime grantedAt, CancellationToken cancellationToken);
    }

    public class UserConsentRepository(IMongoCollection<UserConsentEntity> _userConsents) : IUserConsentRepository
    {
        public async Task<UserConsentEntity?> GetByUserIdAndClientIdAsync(String userId, String  clientId, CancellationToken cancellationToken)
        {
            return await _userConsents.Find(eConsent => eConsent.UserId == userId && eConsent.ClientId == clientId)
              .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(UserConsentEntity userConsentEntity, CancellationToken cancellationToken)
        {
            await _userConsents.InsertOneAsync(userConsentEntity, cancellationToken: cancellationToken);
        }

        public async Task UpdateGrantedScopesAsync(String userId, String clientId, List<String> grantedScopes, DateTime grantedAt, CancellationToken cancellationToken)
        {
            var filter = Builders<UserConsentEntity>.Filter.Eq(x => x.UserId, userId)
                       & Builders<UserConsentEntity>.Filter.Eq(x => x.ClientId, clientId);

            var update = Builders<UserConsentEntity>.Update
                .Set(x => x.GrantedScopes, grantedScopes)
                .Set(x => x.GrantedAt, grantedAt)
                .Set(x => x.ExpiresAt, grantedAt.AddMonths(1));

            await _userConsents.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        }
    }
}
