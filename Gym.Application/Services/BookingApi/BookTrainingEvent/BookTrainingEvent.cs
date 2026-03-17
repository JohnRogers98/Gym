using Gym.Application.Aspects;
using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Services.BookingApi.BookTrainingEvent
{
    public record BookTrainingEvent(String UserId, String CalendarEventId) : IRequest<Result<BookTrainingEventResult>>, ILockedRequest, ITransactionalRequest
    {
        public String GetLockId() => CalendarEventId;

        public String GetLockOperation() => nameof(BookTrainingEvent);
    }
}
