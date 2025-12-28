using Gym.Domain.TrainingAggregate;
using Gym.Infrastructure.Entities.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Repositories.Trainings 
{
    internal class TrainingRepository(IMongoCollection<TrainingEntity> _trainingCollection) : ITrainingRepository, ITrainingQueryService
    {
        public TrainingId NextIdentity() => TrainingId.From(ObjectId.GenerateNewId().ToString());

        public async Task SaveAsync(Training training, CancellationToken cancellationToken)
        {
            TrainingEntity trainingEntity = training.ToEntity();

            await _trainingCollection.ReplaceOneAsync(eTraining => eTraining.Id == training.Id.Value.ToObjectId(),
                trainingEntity,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
        }

        public async Task<Training?> GetByIdAsync(TrainingId id, CancellationToken cancellationToken)
        {
            var foundedEntity = await _trainingCollection.Find(eTraining => eTraining.Id == id.Value.ToObjectId())
                .FirstOrDefaultAsync(cancellationToken);

            return foundedEntity?.ToDomain();
        }

        public async Task<IEnumerable<Training>> GetAllAsync(CancellationToken cancellationToken)
        {
            List<Training> allTrainigs = new();

            await _trainingCollection.Find(Builders<TrainingEntity>.Filter.Empty)
                .ForEachAsync(eTraining => allTrainigs.Add(eTraining.ToDomain()));

            return allTrainigs;
        }

        public async Task<Boolean> ExistsAsync(TrainingId id, CancellationToken cancellationToken) 
            => await _trainingCollection.Find(eTraining => eTraining.Id == id.Value.ToObjectId()).AnyAsync(cancellationToken);

    }
}
