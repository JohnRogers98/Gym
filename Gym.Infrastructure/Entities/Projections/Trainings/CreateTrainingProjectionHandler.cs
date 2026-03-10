using Gym.Abstractions.Query.Trainings;
using Gym.Domain.TrainingContext;
using Gym.Domain.TrainingContext.Events;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.DtoDeserializers;
using Gym.Infrastructure.Entities.Extensions;
using Gym.Infrastructure.Entities.Repositories.Trainings;
using Gym.Infrastructure.Entities.Repositories.Trainings.EventsDto;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.Trainings
{
    internal class CreateTrainingProjectionHandler(
        IMongoCollection<TrainingEntity> _trainingColletion,
        IMongoCollection<TrainingProjection> _projectionCollection,
        MongoUnitOfWork _mongoUnitOfWork,
        IEventDtoDeserializer _eventDtoDeserializer) : IProjectionHandler
    {
        public Boolean CanHandle(String aggregateType, String operation)
        {
            return aggregateType == nameof(Training) && operation == nameof(TrainingCreatedDomainEvent);
        }

        public async Task HandleAsync(EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var trainingCreatedDto = _eventDtoDeserializer.Deserialize<TrainingCreatedDto>(eventEntity);

            var trainingEntity = await _trainingColletion
                .Find(training => training.Id == trainingCreatedDto.TrainingId.ToObjectId())
                .FirstAsync(cancellationToken);

            var projection = new TrainingProjection(
                Id: trainingEntity.Id.ToString(),
                Name: trainingEntity.Name,
                Description: trainingEntity.Description
            );

            await _projectionCollection.InsertOneAsync(_mongoUnitOfWork.Session, projection, cancellationToken: cancellationToken);
        }
    }
}
