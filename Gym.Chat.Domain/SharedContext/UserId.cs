namespace Gym.Chat.Domain.SharedContext
{
    public record UserId
    {
        public String Value { get; }

        private UserId(String value) => Value = value;

        public static UserId From(String value)
        {
            return new(value);
        }

        public override String ToString() => Value.ToString();
    }
}
