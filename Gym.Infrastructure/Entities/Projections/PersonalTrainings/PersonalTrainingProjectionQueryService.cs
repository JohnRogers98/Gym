using Gym.Abstractions.Query.PersonalTrainings;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.PersonalTrainings
{
    internal class PersonalTrainingProjectionQueryService(IMongoCollection<PersonalTrainingProjection> _projectionCollection) : IPersonalTrainingProjectionQueryService
    {
        public async Task<IEnumerable<PersonalTrainingProjection>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _projectionCollection.Find(Builders<PersonalTrainingProjection>.Filter.Empty)
             .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<PersonalTrainingProjection>> GetAllByClientIdAsync(String clientId, CancellationToken cancellationToken)
        {
            return await _projectionCollection.Find(projection => projection.Client.Id == clientId)
                 .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<PersonalTrainingProjection>> GetAllByInstructorIdAsync(String instructorId, CancellationToken cancellationToken)
        {
            return await _projectionCollection.Find(projection => projection.Instructor.Id == instructorId)
                .ToListAsync(cancellationToken);
        }

        public async Task<PersonalTrainingProjection?> GetByIdAsync(String personalTrainingId, CancellationToken cancellationToken)
        {
            return await _projectionCollection.Find(projection => projection.Id == personalTrainingId)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
