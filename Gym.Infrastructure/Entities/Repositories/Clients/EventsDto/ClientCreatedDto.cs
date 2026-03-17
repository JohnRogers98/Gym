using Gym.Domain.ClientContext.Events;
using Gym.Infrastructure.Scanners;

namespace Gym.Infrastructure.Entities.Repositories.Clients.EventsDto
{
    [EventSerializationForm<ClientCreatedDomainEvent>]
    internal record ClientCreatedDto(String Id, DateTime occurredOn, String UserId);
}
