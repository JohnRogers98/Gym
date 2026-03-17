using Gym.Domain._Common;

namespace Gym.Domain.CalendarEventContext.Errors
{
    public class CalendarEventIdValidationError : DomainError
    {
        private CalendarEventIdValidationError() : base(nameof(CalendarEventIdValidationError)) { }

        public static CalendarEventIdValidationError Create() => new();

        public override String GetErrorMessage() => $"Calendar event id is invalid.";
    }
}
