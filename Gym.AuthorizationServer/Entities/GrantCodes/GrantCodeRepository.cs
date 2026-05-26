using MongoDB.Driver;

namespace Gym.AuthorizationServer.Entities.GrantCodes
{
    public interface IGrantCodeRepository
    {
        Task AddAsync(GrantCodeEntity entity, CancellationToken cancellationToken);
        Task<GrantCodeEntity?> GetByCodeAsync(String code, CancellationToken cancellationToken);

        Task<Boolean> DeleteByCodeAsync(String code, CancellationToken cancellationToken);
        Task<GrantCodeEntity?> ConsumeByCodeAsync(String code, CancellationToken cancellationToken);
    }

    public class GrantCodeRepository(IMongoCollection<GrantCodeEntity> _grantCodes) : IGrantCodeRepository
    {
        public async Task AddAsync(GrantCodeEntity grantCodeEntity, CancellationToken cancellationToken)
        {
            await _grantCodes.InsertOneAsync(grantCodeEntity, cancellationToken: cancellationToken);
        }

        public async Task<GrantCodeEntity?> GetByCodeAsync(String code, CancellationToken cancellationToken)
        {
            return await _grantCodes.Find(eGrantCode => eGrantCode.Code == code)
              .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Boolean> DeleteByCodeAsync(String code, CancellationToken cancellationToken)
        {
            var result = await _grantCodes.DeleteOneAsync(x => x.Code == code, cancellationToken);
            return result.DeletedCount > 0;
        }

        public async Task<GrantCodeEntity?> ConsumeByCodeAsync(String code, CancellationToken cancellationToken)
        {
            return await _grantCodes.FindOneAndDeleteAsync(
                eAccessToken => eAccessToken.Code == code, cancellationToken: cancellationToken);
        }
    }
}
