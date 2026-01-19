namespace Gym.Domain.BookingAggregate
{
    public class BookingId
    {
        public String Value { get; }

        private BookingId(String value) => Value = value;

        public static BookingId From(String value) => new(value);

        public override String ToString() => Value.ToString();
    }
}
