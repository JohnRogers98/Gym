using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext.ValueObjects;

namespace Gym.Domain.CalendarEventContext.Errors
{
    public class UserAlreadyBookedError : DomainError
    {
        public CalendarEventId CalendarEventId { get; }
        public UserId UserId { get; }

        private UserAlreadyBookedError(CalendarEventId calendarEventId, UserId userId) : base(nameof(UserAlreadyBookedError))
        {
            CalendarEventId = calendarEventId;
            UserId = userId;
        }

        public static UserAlreadyBookedError Create(CalendarEventId calendarEventId, UserId userId) => new(calendarEventId, userId);

        public override String GetErrorMessage() => $"User - {UserId} already booked for event - {CalendarEventId}";
    }
}
