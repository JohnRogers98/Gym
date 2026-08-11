using Gym.Domain._Common;

namespace Gym.Domain._Shared
{
    public record EndsAt
    {
        public DateTime Value { get; }

        private EndsAt(DateTime value) => Value = value;

        public static Result<EndsAt> From(DateTime value)
        {
            return Result<EndsAt>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
