namespace Gym.Domain.UserAggregate
{
    public class UserId
    {
        public String Value { get; }

        private UserId(String value) => Value = value;

        public static UserId From(String value) => new(value);

        public override String ToString() => Value.ToString();
    }
}
