namespace Gym.Domain.Users
{
    public record TelegramUserId
    {
        public Int64 Value { get; }

        private TelegramUserId(Int64 value) => Value = value;
        public static TelegramUserId From(Int64 value) => new(value);

        public override String ToString() => Value.ToString();
    }
}
