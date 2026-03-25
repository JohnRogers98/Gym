namespace Gym.Domain.PollContext.ValueObjects
{
    public record IsResponseRequired
    {
        public Boolean Value { get; }

        private IsResponseRequired(Boolean value) => Value = value;

        public static IsResponseRequired From(Boolean value)
        {
            return new(value);
        }

        public override String ToString() => Value.ToString();
    }
}
