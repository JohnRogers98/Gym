using Gym.Domain._Common;
using Gym.Domain.PollContext.Errors;

namespace Gym.Domain.PollContext.ValueObjects
{
    public record PollId
    {
        public String Value { get; }

        private PollId(String value) => Value = value;

        public static Result<PollId> From(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Result<PollId>.Fail(PollIdValidationError.Create());
            }

            return Result<PollId>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
