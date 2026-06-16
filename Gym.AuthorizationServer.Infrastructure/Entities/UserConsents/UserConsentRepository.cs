using Gym.AuthorizationServer.Infrastructure.Session;
using MongoDB.Driver;

namespace Gym.AuthorizationServer.Infrastructure.Entities.UserConsents
{
    public interface IUserConsentRepository
    {
        Task<UserConsentEntity?> GetAsync(String userId, String clientId, String protectedResourceId, CancellationToken cancellationToken);
        Task AddAsync(UserConsentEntity entity, CancellationToken cancellationToken);
        Task UpdateGrantedScopesAsync(String userId, String clientId, List<ScopeInfo> grantedScopes, DateTime grantedAt, CancellationToken cancellationToken);
    }

    public class UserConsentRepository(IMongoCollection<UserConsentEntity> _userConsents, MongoUnitOfWork _mongoUnitOfWork) : IUserConsentRepository
    {
        public async Task<UserConsentEntity?> GetAsync(String userId, String  clientId, String protectedResourceId, CancellationToken cancellationToken)
        {
            return await _userConsents
                .Find(_mongoUnitOfWork.Session, eConsent => eConsent.UserId == userId && eConsent.ClientId == clientId && eConsent.ProtectedResourceId == protectedResourceId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(UserConsentEntity userConsentEntity, CancellationToken cancellationToken)
        {
            await _userConsents.InsertOneAsync(_mongoUnitOfWork.Session, userConsentEntity, cancellationToken: cancellationToken);
        }

        public async Task UpdateGrantedScopesAsync(String userId, String clientId, List<ScopeInfo> grantedScopes, DateTime grantedAt, CancellationToken cancellationToken)
        {
            var filter = Builders<UserConsentEntity>.Filter.Eq(x => x.UserId, userId)
                       & Builders<UserConsentEntity>.Filter.Eq(x => x.ClientId, clientId);

            var update = Builders<UserConsentEntity>.Update
                .Set(x => x.GrantedScopes, grantedScopes)
                .Set(x => x.GrantedAt, grantedAt)
                .Set(x => x.ExpiresAt, grantedAt.AddMonths(1));

            await _userConsents.UpdateOneAsync(_mongoUnitOfWork.Session, filter, update, cancellationToken: cancellationToken);
        }
    }
}
