namespace Gym.Domain.UserContext
{
    public record LastName
    {
        public String Value { get; }

        private LastName(String value) => Value = value;
        public static LastName From(String value) => new(value);

        public override String ToString() => Value.ToString();
    }
}
