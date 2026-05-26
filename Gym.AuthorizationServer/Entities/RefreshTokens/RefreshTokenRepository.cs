using MongoDB.Driver;

namespace Gym.AuthorizationServer.Entities.RefreshTokens
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshTokenEntity entity, CancellationToken cancellationToken);
        Task<RefreshTokenEntity?> GetByTokenAsync(String token, CancellationToken cancellationToken);
        Task<Boolean> DeleteByTokenAsync(String token, CancellationToken cancellationToken);
        Task<RefreshTokenEntity?> ConsumeByTokenAsync(String token, CancellationToken cancellationToken);
    }

    public class RefreshTokenRepository(IMongoCollection<RefreshTokenEntity> _refreshTokens) : IRefreshTokenRepository
    {
        public async Task AddAsync(RefreshTokenEntity entity, CancellationToken cancellationToken)
        {
            await _refreshTokens.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }

        public async Task<RefreshTokenEntity?> GetByTokenAsync(String token, CancellationToken cancellationToken)
        {
            return await _refreshTokens.Find(eRefreshToken => eRefreshToken.Token == token)
              .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Boolean> DeleteByTokenAsync(String token, CancellationToken cancellationToken)
        {
            var result = await _refreshTokens.DeleteOneAsync(eRefreshToken => eRefreshToken.Token == token, cancellationToken: cancellationToken);
            return result.DeletedCount > 0;
        }

        public async Task<RefreshTokenEntity?> ConsumeByTokenAsync(String token, CancellationToken cancellationToken)
        {
            return await _refreshTokens.FindOneAndDeleteAsync(
                eRefreshToken => eRefreshToken.Token == token,cancellationToken: cancellationToken);
        }
    }
}
