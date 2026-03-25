using Gym.Domain.PollResponseContext.Events;
using Gym.Infrastructure.Entities.Repositories.PollResponses.EventsDto;

namespace Gym.Infrastructure.Entities.EventStores.Serializers
{
    internal partial class EventSerializer
    {
        private CalendarEventPollResponseCreatedDto ToDto(CalendarEventPollResponseCreatedDomainEvent domainEvent)
        {
            return new CalendarEventPollResponseCreatedDto(
                domainEvent.Id.Value.ToString(),
                domainEvent.OccurredOn,
                domainEvent.PollId.Value,
                domainEvent.PollResponseId.Value);
        }
    }
}
