namespace Gym.Domain.PollContext.ValueObjects
{
    public record CanAcceptManyChoices
    {
        public Boolean Value { get; }

        private CanAcceptManyChoices(Boolean value) => Value = value;

        public static CanAcceptManyChoices From(Boolean value)
        {
            return new(value);
        }

        public override String ToString() => Value.ToString();
    }
}
