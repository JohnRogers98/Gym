using Gym.Abstractions.Query.Clients;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.Clients
{
    internal class ClientProjectionQueryService(IMongoCollection<ClientProjection> _projectionCollection) : IClientProjectionQueryService
    {
        public async Task<IEnumerable<ClientProjection>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _projectionCollection.Find(Builders<ClientProjection>.Filter.Empty)
            .ToListAsync(cancellationToken);
        }

        public async Task<ClientProjection?> GetByIdAsync(String clientId, CancellationToken cancellationToken)
        {
            return await _projectionCollection.Find(projection => projection.Id == clientId)
             .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ClientProjection?> GetByUserIdAsync(String userId, CancellationToken cancellationToken)
        {
            return await _projectionCollection.Find(projection => projection.UserId == userId)
              .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
