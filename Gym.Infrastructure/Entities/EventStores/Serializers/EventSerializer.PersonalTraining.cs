using Gym.Domain.PersonalTrainingContext.Events;
using Gym.Infrastructure.Entities.Repositories.PersonalTrainings.EventsDto;

namespace Gym.Infrastructure.Entities.EventStores.Serializers
{
    internal partial class EventSerializer
    {
        private PersonalTrainingCreatedDto ToDto(PersonalTrainingCreatedDomainEvent domainEvent)
        {
            return new PersonalTrainingCreatedDto(
                domainEvent.Id.Value.ToString(),
                domainEvent.OccurredOn,
                domainEvent.PersonalTrainingId.Value,
                domainEvent.InstructorId.Value,
                domainEvent.ClientId.Value);
        }
    }
}
