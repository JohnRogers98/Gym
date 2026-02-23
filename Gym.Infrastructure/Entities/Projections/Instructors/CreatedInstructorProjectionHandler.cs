using Gym.Abstractions.Query.Instructors;
using Gym.Domain.InstructorContext;
using Gym.Domain.InstructorContext.Events;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.DtoDeserializers;
using Gym.Infrastructure.Entities.Extensions;
using Gym.Infrastructure.Entities.Repositories.Instructors;
using Gym.Infrastructure.Entities.Repositories.Instructors.EventsDto;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.Instructors
{
    internal class CreatedInstructorProjectionHandler(
        IMongoCollection<InstructorEntity> _instructorColletion,
        IMongoCollection<InstructorProjection> _projectionCollection,
        MongoUnitOfWork _mongoUnitOfWork,
        IEventDtoDeserializer _eventDtoDeserializer) : IProjectionHandler
    {
        public Boolean CanHandle(String aggregateType, String operation)
        {
            return aggregateType == nameof(Instructor) && operation == nameof(InstructorCreatedDomainEvent);
        }

        public async Task HandleAsync(EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var instructorCreatedDto = _eventDtoDeserializer.Deserialize<InstructorCreatedDto>(eventEntity);

            var instructorEntity = await _instructorColletion
                .Find(instructor => instructor.Id == instructorCreatedDto.InstructorId.ToObjectId())
                .FirstAsync(cancellationToken);

            var projection = new InstructorProjection(
                Id: instructorEntity.Id.ToString(),
                FullName: String.Concat(instructorEntity.FirstName, " ", instructorEntity.LastName)
            );

            await _projectionCollection.InsertOneAsync(_mongoUnitOfWork.Session, projection, cancellationToken: cancellationToken);
        }
    }
}
