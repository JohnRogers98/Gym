using Gym.Domain._Common;
using Gym.Domain.TrainingContext.Errors;

namespace Gym.Domain.TrainingContext.ValueObjects
{
    public record TrainingName
    {
        public String Value { get; }

        private TrainingName(String value) => Value = value;

        public static Result<TrainingName> From(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Result<TrainingName>.Fail(TrainingNameValidationError.Create());
            }

            return Result<TrainingName>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
