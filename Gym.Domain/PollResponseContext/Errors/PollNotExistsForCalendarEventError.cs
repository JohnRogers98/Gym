using Gym.Domain._Common;
using Gym.Domain.CalendarEventContext.ValueObjects;


namespace Gym.Domain.PollResponseContext.Errors
{
    public class PollNotExistsForCalendarEventError : DomainError
    {
        public CalendarEventId CalendarEventId { get; }
        public PollResponse PollResponse { get; }

        private PollNotExistsForCalendarEventError(CalendarEventId calendarEventId, PollResponse pollResponse) : base(nameof(PollNotExistsForCalendarEventError)) 
        {
            CalendarEventId = calendarEventId;
            PollResponse = pollResponse;
        }

        public static PollNotExistsForCalendarEventError Create(CalendarEventId calendarEventId, PollResponse pollResponse) => new(calendarEventId, pollResponse);

        public override String GetErrorMessage() => $"Calendar event id - {CalendarEventId.Value} has no polling.";
    }
}
