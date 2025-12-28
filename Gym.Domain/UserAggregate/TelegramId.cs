namespace Gym.Domain.UserAggregate
{
    public record TelegramId
    {
        public Int64 Value { get; }

        private TelegramId(Int64 value) => Value = value;
        public static TelegramId From(Int64 value) => new(value);

        public override String ToString() => Value.ToString();
    }
}
