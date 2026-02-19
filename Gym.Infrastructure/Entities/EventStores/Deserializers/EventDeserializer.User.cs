using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.UserContext.Events;
using Gym.Infrastructure.Entities.Repositories.Users.EventsDto;

namespace Gym.Infrastructure.EventStores.Deserializers
{
    internal partial class EventDeserializer
    {
        private DomainEvent ToDomainEvent(UserCreatedDto dto)
        {
            return UserCreatedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.OccuredOn,
                UserId.From(dto.UserId));
        }
    }
}
