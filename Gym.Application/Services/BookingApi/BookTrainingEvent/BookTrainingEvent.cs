using Gym.Application.Aspects;
using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Services.BookingApi.BookTrainingEvent
{
    public record BookTrainingEvent(String ClientId, String CalendarEventId, CalendarEventPollResponse? PollResponse = null) : IRequest<Result<BookTrainingEventResult>>, ILockedRequest, ITransactionalRequest
    {
        public String GetLockId() => CalendarEventId;

        public String GetLockOperation() => nameof(BookTrainingEvent);
    }

    public record CalendarEventPollResponse(String PollId, IEnumerable<Int32> SelectedChoices);
}
