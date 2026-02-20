using Gym.Domain._Common;
using Gym.Domain.InstructorContext;
using Gym.Domain.InstructorContext.Events;
using Gym.Infrastructure.Entities.Repositories.Instructors.EventsDto;

namespace Gym.Infrastructure.Entities.EventStores.Deserializers
{
    internal partial class EventDeserializer
    {
        private DomainEvent ToDomainEvent(InstructorCreatedDto dto)
        {
            return InstructorCreatedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.occurredOn,
                InstructorId.From(dto.InstructorId));
        }
    }
}
