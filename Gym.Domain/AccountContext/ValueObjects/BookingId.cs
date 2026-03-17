using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext.ValueObjects;

namespace Gym.Domain.AccountContext.ValueObjects
{
    public record BookingId
    {
        public String Value { get; }

        private BookingId(String value) => Value = value;

        public static BookingId From(String value) => new(value);

        public static BookingId From(UserId userId, CalendarEventId calendarEventId) => new($"{userId.Value}_{calendarEventId.Value}");

        public override String ToString() => Value.ToString();
    }
}
