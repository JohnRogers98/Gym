using Gym.Domain._Shared;

namespace Gym.Domain.AccountContext
{
    public record BookingId
    {
        public String Value { get; }

        private BookingId(String value) => Value = value;

        public static BookingId From(String value) => new(value);

        public static BookingId From(UserId userId, CalendarEventId calendarEventId) => new($"{userId}_{calendarEventId}");

        public override String ToString() => Value;
    }
}
