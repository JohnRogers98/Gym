namespace Gym.Domain.CalendarEventAggregate
{
    public record CalendarEventId
    {
        public String Value { get; }

        private CalendarEventId(String value) => Value = value;

        public static CalendarEventId From(String value) => new(value);

        public override String ToString() => Value.ToString();
    }
}
