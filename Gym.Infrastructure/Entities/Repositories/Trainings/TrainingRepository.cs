using Gym.Domain.TrainingContext;
using Gym.Infrastructure.Entities.Extensions;
using Gym.Infrastructure.Entities.Extensions.Mappings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Repositories.Trainings 
{
    internal class TrainingRepository(IMongoCollection<TrainingEntity> _trainingCollection, MongoUnitOfWork _mongoUnitOfWork) : ITrainingRepository
    {
        public TrainingId NextIdentity() => TrainingId.From(ObjectId.GenerateNewId().ToString());

        public async Task SaveAsync(Training training, CancellationToken cancellationToken)
        {
            TrainingEntity trainingEntity = training.ToEntity();

            await _trainingCollection.ReplaceOneAsync(
                _mongoUnitOfWork.Session,
                eTraining => eTraining.Id == training.Id.Value.ToObjectId(),
                trainingEntity,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
        }

        public async Task<Training?> GetByIdAsync(TrainingId id, CancellationToken cancellationToken)
        {
            var foundedEntity = await _trainingCollection.Find(_mongoUnitOfWork.Session, eTraining => eTraining.Id == id.Value.ToObjectId())
                .FirstOrDefaultAsync(cancellationToken);

            return foundedEntity?.ToDomain();
        }

        public async Task<Boolean> ExistsAsync(TrainingId id, CancellationToken cancellationToken) 
            => await _trainingCollection.Find(_mongoUnitOfWork.Session, eTraining => eTraining.Id == id.Value.ToObjectId()).AnyAsync(cancellationToken);

    }
}
