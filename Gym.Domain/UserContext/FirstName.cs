namespace Gym.Domain.UserContext
{
    public record FirstName
    {
        public String Value { get; }

        private FirstName(String value) => Value = value;
        public static FirstName From(String value) => new(value);

        public override String ToString() => Value.ToString();
    }
}
