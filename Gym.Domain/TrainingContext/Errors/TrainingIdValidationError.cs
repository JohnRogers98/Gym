using Gym.Domain._Common;

namespace Gym.Domain.TrainingContext.Errors
{
    public class TrainingIdValidationError : DomainError
    {
        private TrainingIdValidationError() : base(nameof(TrainingIdValidationError)) { }

        public static TrainingIdValidationError Create() => new();

        public override String GetErrorMessage() => $"Training id is invalid.";
    }
}
