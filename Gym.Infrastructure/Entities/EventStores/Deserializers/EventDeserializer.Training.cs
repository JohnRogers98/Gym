using Gym.Domain._Common;
using Gym.Domain.TrainingContext;
using Gym.Domain.TrainingContext.Events;
using Gym.Infrastructure.Entities.Repositories.Trainings.EventsDto;

namespace Gym.Infrastructure.Entities.EventStores.Deserializers
{
    internal partial class EventDeserializer
    {
        private DomainEvent ToDomainEvent(TrainingCreatedDto dto)
        {
            return TrainingCreatedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.occurredOn,
                TrainingId.From(dto.TrainingId));
        }
    }
}
