using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.InstructorContext.Errors;

namespace Gym.Domain.InstructorContext.ValueObjects
{
    public record InstructorId
    {
        public String Value { get; }

        private InstructorId(String value) => Value = value;

        public static Result<InstructorId> From(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Result<InstructorId>.Fail(InstructorIdValidationError.Create());
            }

            return Result<InstructorId>.Ok(new(value));
        }

        public static InstructorId From(UserId userId)
        {
            return new(userId.Value);
        }

        public override String ToString() => Value.ToString();
    }
}
