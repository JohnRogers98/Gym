using Gym.Domain._Common;
using Gym.Domain.PollContext.Errors;

namespace Gym.Domain.PollContext.ValueObjects
{
    public record ChoiceText
    {
        public String Value { get; }

        private ChoiceText(String value) => Value = value;

        public static Result<ChoiceText> From(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Result<ChoiceText>.Fail(ChoiceTextValidationError.Create());
            }

            return Result<ChoiceText>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
