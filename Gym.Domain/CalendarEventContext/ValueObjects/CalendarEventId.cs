using Gym.Domain._Common;
using Gym.Domain.CalendarEventContext.Errors;

namespace Gym.Domain.CalendarEventContext.ValueObjects
{
    public record CalendarEventId
    {
        public String Value { get; }

        private CalendarEventId(String value) => Value = value;

        public static Result<CalendarEventId> From(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Result<CalendarEventId>.Fail(CalendarEventIdValidationError.Create());
            }

            return Result<CalendarEventId>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
