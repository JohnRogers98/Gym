using Gym.Domain.PersonalTrainingContext.Events;
using Gym.Infrastructure.Scanners;

namespace Gym.Infrastructure.Entities.Repositories.PersonalTrainings.EventsDto
{
    [EventSerializationForm<PersonalTrainingCancelledDomainEvent>]
    internal record PersonalTrainingCancelledDto(String Id, DateTime occurredOn, String PersonalTrainingId, String InstructorId, String ClientId);
}
