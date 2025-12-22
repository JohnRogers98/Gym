namespace Gym.Domain.InstructorAggregate
{
    public record InstructorId
    {
        public String Value { get; }

        private InstructorId(String value) => Value = value;

        public static InstructorId From(String value) => new(value);

        public override String ToString() => Value.ToString();
    }
}
