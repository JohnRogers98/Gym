using Gym.Domain._Common;
using Gym.Domain.PollContext.Errors;

namespace Gym.Domain.PollContext.ValueObjects
{
    public record ChoiceId
    {
        public Int32 Value { get; }

        private ChoiceId(Int32 value) => Value = value;

        public static Result<ChoiceId> From(Int32 value)
        {
            if (value < 0 && value >= 5)
            {
                return Result<ChoiceId>.Fail(ChoiseIdValidationError.Create());
            }

            return Result<ChoiceId>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
