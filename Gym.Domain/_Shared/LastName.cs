using Gym.Domain._Common;
using Gym.Domain._Shared.Errors;

namespace Gym.Domain._Shared
{
    public record LastName
    {
        public String Value { get; }

        private LastName(String value) => Value = value;

        public static Result<LastName> From(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Result<LastName>.Fail(LastNameValidationError.Create());
            }

            return Result<LastName>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
