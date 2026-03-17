using Gym.Domain.TrainingContext.Events;
using Gym.Infrastructure.Scanners;

namespace Gym.Infrastructure.Entities.Repositories.Trainings.EventsDto
{
    [EventSerializationForm<TrainingCreatedDomainEvent>]
    internal record TrainingCreatedDto(String Id, DateTime occurredOn, String TrainingId);
}
