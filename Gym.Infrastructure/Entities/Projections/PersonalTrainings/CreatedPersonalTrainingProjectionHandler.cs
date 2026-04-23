using Gym.Abstractions.Query._CommonInfos;
using Gym.Abstractions.Query.PersonalTrainings;
using Gym.Domain.PersonalTrainingContext;
using Gym.Domain.PersonalTrainingContext.Events;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.DtoDeserializers;
using Gym.Infrastructure.Entities.Extensions;
using Gym.Infrastructure.Entities.Repositories.PersonalTrainings;
using Gym.Infrastructure.Entities.Repositories.PersonalTrainings.EventsDto;
using Gym.Infrastructure.Entities.Repositories.Users;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.PersonalTrainings
{
    internal class CreatedPersonalTrainingProjectionHandler(
        IMongoCollection<PersonalTrainingEntity> _personalTrainingCollection,
        IMongoCollection<PersonalTrainingProjection> _projectionCollection,
        IMongoCollection<UserEntity> _userCollection,
        MongoUnitOfWork _mongoUnitOfWork,
        IEventDtoDeserializer _eventDtoDeserializer) : IProjectionHandler
    {
        public Boolean CanHandle(String aggregateType, String operation)
        {
            return aggregateType == nameof(PersonalTraining) && operation == nameof(PersonalTrainingCreatedDomainEvent);
        }

        public async Task HandleAsync(EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var personalTrainingCreatedDto = _eventDtoDeserializer.Deserialize<PersonalTrainingCreatedDto>(eventEntity);

            var instructorUserEntity = await _userCollection
                .Find(user => user.Id == personalTrainingCreatedDto.InstructorId.ToObjectId())
                .FirstAsync(cancellationToken);

            var clientUserEntity = await _userCollection
                .Find(user => user.Id == personalTrainingCreatedDto.ClientId.ToObjectId())
                .FirstAsync(cancellationToken);

            var personalTrainingEntity = await _personalTrainingCollection
                .Find(personalTraining => personalTraining.Id == personalTrainingCreatedDto.PersonalTrainingId.ToObjectId())
                .FirstAsync(cancellationToken);

            var projection = new PersonalTrainingProjection(
                Id: personalTrainingEntity.Id.ToString(),
                Instructor: new InstructorInfo(instructorUserEntity.Id.ToString(), String.Concat(instructorUserEntity.FirstName, " ", instructorUserEntity.LastName)),
                Client: new ClientInfo(clientUserEntity.Id.ToString(), String.Concat(clientUserEntity.FirstName, " ", clientUserEntity.LastName)),
                Status: personalTrainingEntity.Status,
                Start: personalTrainingEntity.Start,
                End: personalTrainingEntity.End,
                PaymentStatus:  personalTrainingEntity.PaymentStatus,
                InstructorComment: personalTrainingEntity.InstructorComment,
                ClientComment: personalTrainingEntity.ClientComment
            );

            await _projectionCollection.InsertOneAsync(_mongoUnitOfWork.Session, projection, cancellationToken: cancellationToken);
        }
    }
}
