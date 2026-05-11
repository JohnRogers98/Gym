using Gym.Abstractions.Query.PersonalTrainings;
using Gym.Domain.PersonalTrainingContext;
using Gym.Domain.PersonalTrainingContext.Events;
using Gym.Domain.PersonalTrainingContext.ValueObjects;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.DtoDeserializers;
using Gym.Infrastructure.Entities.Repositories.PersonalTrainings.EventsDto;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.PersonalTrainings
{
    internal class CancelledPersonalTrainingProjectionHandler(
        IMongoCollection<PersonalTrainingProjection> _projectionCollection,
        MongoUnitOfWork _mongoUnitOfWork,
        IEventDtoDeserializer _eventDtoDeserializer) : IProjectionHandler
    {
        public Boolean CanHandle(String aggregateType, String operation)
        {
            return aggregateType == nameof(PersonalTraining) && operation == nameof(PersonalTrainingCancelledDomainEvent);
        }

        public async Task HandleAsync(EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var personalTrainingCancelledDto = _eventDtoDeserializer.Deserialize<PersonalTrainingCancelledDto>(eventEntity);

            await _projectionCollection.UpdateOneAsync(
               _mongoUnitOfWork.Session,
               projection => projection.Id == personalTrainingCancelledDto.PersonalTrainingId,
               Builders<PersonalTrainingProjection>.Update.Set(x => x.Status, PersonalTrainingStatus.Cancelled.ToString()),
               cancellationToken: cancellationToken);
        }
    }
}
