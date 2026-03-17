using Gym.Application.Extensions;
using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.UserContext.Events;
using Gym.Infrastructure.Entities.Repositories.Users.EventsDto;

namespace Gym.Infrastructure.Entities.EventStores.Deserializers
{
    internal partial class EventDeserializer
    {
        private DomainEvent ToDomainEvent(UserCreatedDto dto)
        {
            return UserCreatedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.occurredOn,
                UserId.From(dto.UserId).Unwrap()
            );
        }
    }
}
