using Gym.Domain._Common;
using Gym.Domain.PollContext.ValueObjects;
using Gym.Domain.PollResponseContext.ValueObjects;

namespace Gym.Domain.PollResponseContext.Events
{
    public class CalendarEventPollResponseCreatedDomainEvent : DomainEvent
    {
        public PollId PollId { get; private set; }
        public PollResponseId PollResponseId { get; private set; }

        private CalendarEventPollResponseCreatedDomainEvent(DomainEventId id, DateTime occurredOn, PollId pollId, PollResponseId pollResponseId)
            : base(id, occurredOn)
            => (PollId, PollResponseId) = (pollId, pollResponseId);

        public static CalendarEventPollResponseCreatedDomainEvent Create(PollId pollId, PollResponseId pollResponseId)
            => new(DomainEventId.Generate(), DateTime.UtcNow, pollId, pollResponseId);

        public static CalendarEventPollResponseCreatedDomainEvent Restore(DomainEventId id, DateTime occurredOn, PollId pollId, PollResponseId pollResponseId)
           => new(id, occurredOn, pollId, pollResponseId);
    }
}
