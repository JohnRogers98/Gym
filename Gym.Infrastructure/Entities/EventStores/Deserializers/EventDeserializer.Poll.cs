using Gym.Application.Extensions;
using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.PollContext.ValueObjects;
using Gym.Domain.PollResponseContext.Events;
using Gym.Domain.PollResponseContext.ValueObjects;
using Gym.Infrastructure.Entities.Repositories.PollResponses.EventsDto;

namespace Gym.Infrastructure.Entities.EventStores.Deserializers
{
    internal partial class EventDeserializer
    {
        private DomainEvent ToDomainEvent(CalendarEventPollResponseCreatedDto dto)
        {
            return CalendarEventPollResponseCreatedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.occurredOn,
                PollId.From(dto.PollId).Unwrap(),
                PollResponseId.From(UserId.From(dto.PollResponseId.Split('_')[0]).Unwrap(), PollId.From(dto.PollId).Unwrap())
            );
        }
    }
}
