using MongoDB.Driver;

namespace Gym.AuthorizationServer.Entities.AccessTokens
{
    public interface IAccessTokenRepository
    {
        Task AddAsync(AccessTokenEntity entity, CancellationToken cancellationToken);
        Task<AccessTokenEntity?> GetByTokenAsync(String token, CancellationToken cancellationToken);
        Task<Boolean> DeleteByTokenAsync(String token, CancellationToken cancellationToken);
        Task<AccessTokenEntity?> ConsumeByTokenAsync(String token, CancellationToken cancellationToken);
        Task<AccessTokenEntity?> ConsumeByIdAsync(String id, CancellationToken cancellationToken);
    }

    public class AccessTokenRepository(IMongoCollection<AccessTokenEntity> _accessTokens) : IAccessTokenRepository
    {
        public async Task AddAsync(AccessTokenEntity entity, CancellationToken cancellationToken)
        {
            await _accessTokens.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }

        public async Task<AccessTokenEntity?> GetByTokenAsync(String token, CancellationToken cancellationToken)
        {
            return await _accessTokens.Find(eAccessToken => eAccessToken.Token == token)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Boolean> DeleteByTokenAsync(String token, CancellationToken cancellationToken)
        {
            var result = await _accessTokens.DeleteOneAsync(eAccessToken => eAccessToken.Token == token, cancellationToken: cancellationToken);
            return result.DeletedCount > 0;
        }

        public async Task<AccessTokenEntity?> ConsumeByTokenAsync(String token, CancellationToken cancellationToken)
        {
            return await _accessTokens.FindOneAndDeleteAsync(
                eAccessToken => eAccessToken.Token == token, cancellationToken: cancellationToken);
        }

        public async Task<AccessTokenEntity?> ConsumeByIdAsync(String id, CancellationToken cancellationToken)
        {
            return await _accessTokens.FindOneAndDeleteAsync(
                eAccessToken => eAccessToken.Id == id, cancellationToken: cancellationToken);
        }
    }
}
