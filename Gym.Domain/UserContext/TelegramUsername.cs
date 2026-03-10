namespace Gym.Domain.UserContext
{
    public record TelegramUsername
    {
        public String Value { get; }

        private TelegramUsername(String value) => Value = value;
        public static TelegramUsername From(String value) => new(value);

        public override String ToString() => Value.ToString();
    }
}
