using Gym.AuthorizationServer.Infrastructure.Session;
using MongoDB.Driver;

namespace Gym.AuthorizationServer.Infrastructure.Entities.ProtectedResources
{
    public interface IProtectedResourceRepository
    {
        Task<ProtectedResourceEntity?> GetByIdAsync(String id, CancellationToken cancellationToken);
        Task<ProtectedResourceEntity?> GetByAudienceUriAsync(String audienceUri, CancellationToken cancellationToken);
        Task AddAsync(ProtectedResourceEntity entity, CancellationToken cancellationToken);
        Task<Boolean> ExistsByAudienceUriAsync(String audienceUri, CancellationToken cancellationToken);
    }

    internal class ProtectedResourceRepository(IMongoCollection<ProtectedResourceEntity> _protectedResources, MongoUnitOfWork _mongoUnitOfWork) 
        : IProtectedResourceRepository
    {
        public async Task AddAsync(ProtectedResourceEntity entity, CancellationToken cancellationToken)
        {
            await _protectedResources.InsertOneAsync(_mongoUnitOfWork.Session, entity, cancellationToken: cancellationToken);
        }

        public async Task<Boolean> ExistsByAudienceUriAsync(String audienceUri, CancellationToken cancellationToken)
        {
            return await _protectedResources.Find(_mongoUnitOfWork.Session, eProtectedResource => eProtectedResource.AudienceUri == audienceUri)
                .AnyAsync(cancellationToken);
        }

        public async Task<ProtectedResourceEntity?> GetByAudienceUriAsync(String audienceUri, CancellationToken cancellationToken)
        {
            return await _protectedResources.Find(_mongoUnitOfWork.Session, eProtectedResource => eProtectedResource.AudienceUri == audienceUri)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ProtectedResourceEntity?> GetByIdAsync(String id, CancellationToken cancellationToken)
        {
            return await _protectedResources.Find(_mongoUnitOfWork.Session, eProtectedResource => eProtectedResource.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
