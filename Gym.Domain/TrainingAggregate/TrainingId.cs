namespace Gym.Domain.TrainingAggregate
{
    public record TrainingId
    {
        public String Value { get; }

        private TrainingId(String value) => Value = value;
        public static TrainingId From(String value) => new(value);

        public override String ToString() => Value.ToString();
    }
}
