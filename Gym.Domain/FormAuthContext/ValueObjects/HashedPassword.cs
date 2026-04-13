using Gym.Domain._Common;
using Gym.Domain.FormAuthContext.Errors;

namespace Gym.Domain.FormAuthContext.ValueObjects
{
    public record HashedPassword
    {
        public String Value { get; }

        private HashedPassword(String value) => Value = value;

        public static Result<HashedPassword> From(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Result<HashedPassword>.Fail(PasswordValidationError.Create());
            }

            return Result<HashedPassword>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
