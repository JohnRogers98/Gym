using Gym.Domain._Common;
using Gym.Domain._Shared.Errors;

namespace Gym.Domain._Shared
{
    public record UserId
    {
        public String Value { get; }

        private UserId(String value) => Value = value;

        public static Result<UserId> From(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Result<UserId>.Fail(UserIdValidationError.Create());
            }

            return Result<UserId>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
