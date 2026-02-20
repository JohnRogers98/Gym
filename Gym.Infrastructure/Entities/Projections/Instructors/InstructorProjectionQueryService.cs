using Gym.Abstractions.Query.Instructors;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.Instructors
{
    internal class InstructorProjectionQueryService(IMongoCollection<InstructorProjection> _projectionCollection) : IInstructorProjectionQueryService
    {
        public async Task<IEnumerable<InstructorProjection>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _projectionCollection.Find(Builders<InstructorProjection>.Filter.Empty)
              .ToListAsync(cancellationToken);
        }

        public async Task<InstructorProjection?> GetByIdAsync(String instructorId, CancellationToken cancellationToken)
        {
            return await _projectionCollection.Find(projection => projection.Id == instructorId)
              .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
