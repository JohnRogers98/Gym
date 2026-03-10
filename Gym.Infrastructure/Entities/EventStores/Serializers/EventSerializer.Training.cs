using Gym.Domain.TrainingContext.Events;
using Gym.Infrastructure.Entities.Repositories.Trainings.EventsDto;

namespace Gym.Infrastructure.Entities.EventStores.Serializers
{
    internal partial class EventSerializer
    {
        private TrainingCreatedDto ToDto(TrainingCreatedDomainEvent domainEvent)
        {
            return new TrainingCreatedDto(
                domainEvent.Id.Value.ToString(),
                domainEvent.OccurredOn,
                domainEvent.TrainingId.Value);
        }
    }
}
