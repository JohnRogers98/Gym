using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.PollContext.ValueObjects;
using Gym.Domain.PollResponseContext.Events;
using Gym.Domain.PollResponseContext.ValueObjects;

namespace Gym.Domain.PollResponseContext
{
    public class PollResponse : AggregateRoot
    {
        public PollResponseId Id { get; }

        public UserId UserId { get; }

        public PollId PollId { get; }

        public IReadOnlyCollection<ChoiceId> Choices { get; }

        private PollResponse(UserId userId, PollId pollId, IReadOnlyCollection<ChoiceId> choices)
        {
            Id = PollResponseId.From(userId, pollId);
            UserId = userId;
            PollId = pollId;
            Choices = choices;
        }

        public static PollResponse Create(UserId userId, PollId pollId, IReadOnlyCollection<ChoiceId> choices)
        {
            PollResponse pollResponse = new (userId, pollId, choices);
            pollResponse.AddDomainEvent(CalendarEventPollResponseCreatedDomainEvent.Create(pollResponse.PollId, pollResponse.Id));

            return pollResponse;
        }

        public static PollResponse Restore(UserId userId, PollId pollId, IReadOnlyCollection<ChoiceId> choices)
        {
            return new(userId, pollId, choices);
        }
    }
}
