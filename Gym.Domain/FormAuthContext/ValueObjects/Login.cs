using Gym.Domain._Common;
using Gym.Domain.FormAuthContext.Errors;

namespace Gym.Domain.FormAuthContext.ValueObjects
{
    public record Login
    {
        public String Value { get; }

        private Login(String value) => Value = value;

        public static Result<Login> From(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Result<Login>.Fail(LoginValidationError.Create());
            }

            return Result<Login>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
