using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext.ValueObjects;

namespace Gym.Domain.AccountContext.Errors
{
    public class CalendarEventAlreadyBookedError : DomainError
    {
        public UserId UserId { get; }
        public CalendarEventId CalendarEventId { get; }

        private CalendarEventAlreadyBookedError(UserId userId, CalendarEventId calendarEventId) : base(nameof(CalendarEventAlreadyBookedError))
        {
            UserId = userId;
            CalendarEventId = calendarEventId;
        }

        public static CalendarEventAlreadyBookedError Create(UserId userId, CalendarEventId calendarEventId) => new(userId, calendarEventId);

        public override String GetErrorMessage() => $"Current calendar event - {CalendarEventId} already booked.";
    }
}
