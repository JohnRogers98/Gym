using Gym.Domain._Common;
using Gym.Domain._Shared.Errors;

namespace Gym.Domain._Shared
{
    public record FirstName
    {
        public String Value { get; }

        private FirstName(String value) => Value = value;

        public static Result<FirstName> From(String value) 
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Result<FirstName>.Fail(FirstNameValidationError.Create());
            }

            return Result<FirstName>.Ok(new(value));
        } 

        public override String ToString() => Value.ToString();
    }
}
