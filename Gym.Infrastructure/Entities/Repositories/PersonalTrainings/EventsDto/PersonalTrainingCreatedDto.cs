using Gym.Domain.PersonalTrainingContext.Events;
using Gym.Infrastructure.Scanners;

namespace Gym.Infrastructure.Entities.Repositories.PersonalTrainings.EventsDto
{
    [EventSerializationForm<PersonalTrainingCreatedDomainEvent>]
    internal record PersonalTrainingCreatedDto(String Id, DateTime occurredOn, String PersonalTrainingId, String InstructorId, String ClientId);
}
