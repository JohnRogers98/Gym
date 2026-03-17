using Gym.Domain._Common;

namespace Gym.Domain._Shared
{
    public class Description
    {
        public String Value { get; }

        private Description(String value) => Value = value;

        public static Result<Description> From(String value)
        {
            return Result<Description>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
