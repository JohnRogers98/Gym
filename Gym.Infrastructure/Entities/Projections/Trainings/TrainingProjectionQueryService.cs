using Gym.Abstractions.Query.Trainings;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.Trainings
{
    internal class TrainingProjectionQueryService(IMongoCollection<TrainingProjection> _projectionCollection) : ITrainingProjectionQueryService
    {
        public async Task<IEnumerable<TrainingProjection>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _projectionCollection.Find(Builders<TrainingProjection>.Filter.Empty)
             .ToListAsync(cancellationToken);
        }

        public async Task<TrainingProjection?> GetByIdAsync(String trainingId, CancellationToken cancellationToken)
        {
            return await _projectionCollection.Find(projection => projection.Id == trainingId)
             .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
