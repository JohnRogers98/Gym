namespace Gym.Domain.ClientContext
{
    public class ClientId
    {
        public String Value { get; }

        private ClientId(String value) => Value = value;

        public static ClientId From(String value) => new(value);

        public override String ToString() => Value.ToString();
    }
}
