using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext.ValueObjects;

namespace Gym.Domain.AccountContext.Errors
{
    public class CalendarEventBookingNotExistError : DomainError
    {
        public UserId UserId { get; }
        public CalendarEventId CalendarEventId { get; }

        private CalendarEventBookingNotExistError(UserId userId, CalendarEventId calendarEventId) : base(nameof(CalendarEventBookingNotExistError))
        {
            UserId = userId;
            CalendarEventId = calendarEventId;
        }

        public static CalendarEventBookingNotExistError Create(UserId userId, CalendarEventId calendarEventId) => new(userId, calendarEventId);

        public override String GetErrorMessage() => $"Current calendar event - {CalendarEventId} has no booking.";
    }
}
