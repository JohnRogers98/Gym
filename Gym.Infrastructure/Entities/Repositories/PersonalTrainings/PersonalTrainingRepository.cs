using Gym.Application.Extensions;
using Gym.Domain.PersonalTrainingContext;
using Gym.Domain.PersonalTrainingContext.ValueObjects;
using Gym.Infrastructure.Entities.Extensions;
using Gym.Infrastructure.Entities.Extensions.Mappings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Repositories.PersonalTrainings
{
    internal class PersonalTrainingRepository(IMongoCollection<PersonalTrainingEntity> _personalTrainingCollection, MongoUnitOfWork _mongoUnitOfWork) : IPersonalTrainingRepository
    {
        public PersonalTrainingId NextIdentity() => PersonalTrainingId.From(ObjectId.GenerateNewId().ToString()).Unwrap();

        public async Task<PersonalTraining?> GetByIdAsync(PersonalTrainingId id, CancellationToken cancellationToken)
        {
            var foundedEntity = await _personalTrainingCollection.Find(_mongoUnitOfWork.Session, ePersonalTraining => ePersonalTraining.Id == id.Value.ToObjectId())
              .FirstOrDefaultAsync(cancellationToken);

            return foundedEntity?.ToDomain();
        }

        public async Task SaveAsync(PersonalTraining personalTraining, CancellationToken cancellationToken)
        {
            PersonalTrainingEntity personalTrainingEntity = personalTraining.ToEntity();

            await _personalTrainingCollection.ReplaceOneAsync(
                _mongoUnitOfWork.Session,
                ePersonalTraining => ePersonalTraining.Id == personalTrainingEntity.Id,
                personalTrainingEntity,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
        }

        public async Task<Boolean> ExistsAsync(PersonalTrainingId id, CancellationToken cancellationToken)
            => await _personalTrainingCollection.Find(_mongoUnitOfWork.Session, ePersonalTraining => ePersonalTraining.Id == id.Value.ToObjectId()).AnyAsync(cancellationToken);
    }
}
