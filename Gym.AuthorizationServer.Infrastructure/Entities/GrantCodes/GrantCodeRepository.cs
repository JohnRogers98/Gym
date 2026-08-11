using Gym.AuthorizationServer.Infrastructure.Session;
using MongoDB.Driver;

namespace Gym.AuthorizationServer.Infrastructure.Entities.GrantCodes
{
    public interface IGrantCodeRepository
    {
        Task AddAsync(GrantCodeEntity entity, CancellationToken cancellationToken);
        Task<GrantCodeEntity?> GetByCodeAsync(String code, CancellationToken cancellationToken);

        Task<Boolean> DeleteByCodeAsync(String code, CancellationToken cancellationToken);
        Task<GrantCodeEntity?> ConsumeByCodeAsync(String code, CancellationToken cancellationToken);
    }

    public class GrantCodeRepository(IMongoCollection<GrantCodeEntity> _grantCodes, MongoUnitOfWork _mongoUnitOfWork) : IGrantCodeRepository
    {
        public async Task AddAsync(GrantCodeEntity grantCodeEntity, CancellationToken cancellationToken)
        {
            await _grantCodes.InsertOneAsync(_mongoUnitOfWork.Session, grantCodeEntity, cancellationToken: cancellationToken);
        }

        public async Task<GrantCodeEntity?> GetByCodeAsync(String code, CancellationToken cancellationToken)
        {
            return await _grantCodes.Find(_mongoUnitOfWork.Session, eGrantCode => eGrantCode.Code == code)
              .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Boolean> DeleteByCodeAsync(String code, CancellationToken cancellationToken)
        {
            var result = await _grantCodes
                .DeleteOneAsync(_mongoUnitOfWork.Session, eGrantCode => eGrantCode.Code == code, cancellationToken: cancellationToken);
            return result.DeletedCount > 0;
        }

        public async Task<GrantCodeEntity?> ConsumeByCodeAsync(String code, CancellationToken cancellationToken)
        {
            return await _grantCodes
                .FindOneAndDeleteAsync(_mongoUnitOfWork.Session, eAccessToken => eAccessToken.Code == code, cancellationToken: cancellationToken);
        }
    }
}
