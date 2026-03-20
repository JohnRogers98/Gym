using Gym.Domain.PollResponseContext.Events;
using Gym.Infrastructure.Scanners;

namespace Gym.Infrastructure.Entities.Repositories.PollResponses.EventsDto
{
    [EventSerializationForm<CalendarEventPollResponseCreatedDomainEvent>]
    internal record CalendarEventPollResponseCreatedDto(String Id, DateTime occurredOn, String PollId, String PollResponseId);
}
