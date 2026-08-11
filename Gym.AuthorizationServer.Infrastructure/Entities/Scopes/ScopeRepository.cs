using Gym.AuthorizationServer.Infrastructure.Session;
using MongoDB.Driver;

namespace Gym.AuthorizationServer.Infrastructure.Entities.Scopes
{
    public interface IScopeRepository
    {
        Task<ScopeEntity?> GetByIdAsync(String id, CancellationToken cancellationToken);
        Task<IEnumerable<ScopeEntity>> GetByRoleIdAndProtectedResourceIdAsync(String roleId, String protectedResourceId, CancellationToken cancellationToken);
        Task<IEnumerable<ScopeEntity>> GetByProtectedResourceIdAsync(String protectedResourceId, CancellationToken cancellationToken);
        Task AddAsync(ScopeEntity entity, CancellationToken cancellationToken);
    }

    internal class ScopeRepository(IMongoCollection<ScopeEntity> _scopes, MongoUnitOfWork _mongoUnitOfWork) : IScopeRepository
    {
        public async Task AddAsync(ScopeEntity entity, CancellationToken cancellationToken)
        {
            await _scopes.InsertOneAsync(_mongoUnitOfWork.Session, entity, cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<ScopeEntity>> GetByRoleIdAndProtectedResourceIdAsync(String roleId, String protectedResourceId, CancellationToken cancellationToken)
        {
            return await _scopes.Find(_mongoUnitOfWork.Session, eScope => eScope.RoleId == roleId && eScope.ProtectedResourceId == protectedResourceId)
                .ToListAsync(cancellationToken);
        }

        public async Task<ScopeEntity?> GetByIdAsync(String id, CancellationToken cancellationToken)
        {
            return await _scopes.Find(_mongoUnitOfWork.Session, eUser => eUser.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<ScopeEntity>> GetByProtectedResourceIdAsync(String protectedResourceId, CancellationToken cancellationToken)
        {
            return await _scopes.Find(_mongoUnitOfWork.Session, eScope => eScope.ProtectedResourceId == protectedResourceId)
                .ToListAsync(cancellationToken);
        }
    }
}
