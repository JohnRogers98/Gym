using Gym.Domain._Common;

namespace Gym.Domain.PersonalTrainingContext.Errors
{
    public class PersonalTrainingIdValidationError : DomainError
    {
        private PersonalTrainingIdValidationError() : base(nameof(PersonalTrainingIdValidationError)) { }

        public static PersonalTrainingIdValidationError Create() => new();

        public override String GetErrorMessage() => $"Personal training id is invalid.";
    }
}
