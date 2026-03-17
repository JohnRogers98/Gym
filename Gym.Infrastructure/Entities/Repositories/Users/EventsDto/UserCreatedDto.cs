using Gym.Domain.UserContext.Events;
using Gym.Infrastructure.Scanners;

namespace Gym.Infrastructure.Entities.Repositories.Users.EventsDto
{
    [EventSerializationForm<UserCreatedDomainEvent>]
    internal record UserCreatedDto(String Id, DateTime occurredOn, String UserId);
}
