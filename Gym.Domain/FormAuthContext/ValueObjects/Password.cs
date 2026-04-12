using Gym.Domain._Common;
using Gym.Domain.FormAuthContext.Errors;

namespace Gym.Domain.FormAuthContext.ValueObjects
{
    public record Password
    {
        public String Value { get; }

        private Password(String value) => Value = value;

        public static Result<Password> From(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Result<Password>.Fail(PasswordValidationError.Create());
            }

            return Result<Password>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
