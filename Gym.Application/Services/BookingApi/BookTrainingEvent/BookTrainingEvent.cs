using Gym.Application.Aspects;
using MediatR;

namespace Gym.Application.Services.BookingApi.BookTrainingEvent
{
    public record BookTrainingEvent(String UserId, String CalendarEventId) : IRequest<BookingDetails>, ILockedRequest
    {
        public String GetLockId() => CalendarEventId;

        public String GetLockOperation() => nameof(BookTrainingEvent);
    }
}
