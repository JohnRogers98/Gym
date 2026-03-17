using Gym.Domain._Common;
using Gym.Domain.TrainingContext.Errors;

namespace Gym.Domain.TrainingContext.ValueObjects
{
    public record TrainingId
    {
        public String Value { get; }

        private TrainingId(String value) => Value = value;

        public static Result<TrainingId> From(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Result<TrainingId>.Fail(TrainingIdValidationError.Create());
            }

            return Result<TrainingId>.Ok(new (value)); 
        }

        public override String ToString() => Value.ToString();
    }
}
