using Gym.Domain.ClientContext.Events;
using Gym.Infrastructure.Entities.Repositories.Clients.EventsDto;

namespace Gym.Infrastructure.Entities.EventStores.Serializers
{
    internal partial class EventSerializer
    {
        private ClientCreatedDto ToDto(ClientCreatedDomainEvent domainEvent)
        {
            return new ClientCreatedDto(
                domainEvent.Id.Value.ToString(),
                domainEvent.OccurredOn,
                domainEvent.UserId.Value);
        }
    }
}
