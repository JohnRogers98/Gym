using Gym.Domain.InstructorContext.Events;
using Gym.Infrastructure.Entities.Repositories.Instructors.EventsDto;

namespace Gym.Infrastructure.Entities.EventStores.Serializers
{
    internal partial class EventSerializer
    {
        private InstructorCreatedDto ToDto(InstructorCreatedDomainEvent domainEvent)
        {
            return new InstructorCreatedDto(
                domainEvent.Id.Value.ToString(),
                domainEvent.OccurredOn,
                domainEvent.InstructorId.Value);
        }
    }
}
