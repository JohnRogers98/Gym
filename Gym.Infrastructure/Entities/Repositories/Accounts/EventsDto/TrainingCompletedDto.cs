using Gym.Domain.AccountContext.Events;
using Gym.Infrastructure.Scanners;

namespace Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto
{
    [EventSerializationForm<TrainingCompletedDomainEvent>]
    internal record TrainingCompletedDto(String Id, DateTime OccurredOn, String BookingId, String UserId, String CalendarEventId);
}
