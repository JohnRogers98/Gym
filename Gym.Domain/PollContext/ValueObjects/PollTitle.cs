using Gym.Domain._Common;
using Gym.Domain.PollContext.Errors;

namespace Gym.Domain.PollContext.ValueObjects
{
    public record PollTitle
    {
        public String Value { get; }

        private PollTitle(String value) => Value = value;

        public static Result<PollTitle> From(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Result<PollTitle>.Fail(PollTitleValidationError.Create());
            }

            return Result<PollTitle>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
