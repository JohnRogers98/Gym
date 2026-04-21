using Gym.Domain._Common;
using Gym.Domain.PersonalTrainingContext.Errors;

namespace Gym.Domain.PersonalTrainingContext.ValueObjects
{
    public record PersonalTrainingId
    {
        public String Value { get; }

        private PersonalTrainingId(String value) => Value = value;
        public static Result<PersonalTrainingId> From(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Result<PersonalTrainingId>.Fail(PersonalTrainingIdValidationError.Create());
            }

            return Result<PersonalTrainingId>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
