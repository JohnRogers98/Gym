using Gym.Domain.UserContext.Events;
using Gym.Infrastructure.Entities.Repositories.Users.EventsDto;

namespace Gym.Infrastructure.Entities.EventStores.Serializers
{
    internal partial class EventSerializer
    {
        private UserCreatedDto ToDto(UserCreatedDomainEvent domainEvent)
        {
            return new UserCreatedDto(
                domainEvent.Id.Value.ToString(),
                domainEvent.OccurredOn,
                domainEvent.UserId.Value);
        }
    }
}
