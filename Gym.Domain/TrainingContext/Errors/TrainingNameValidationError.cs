using Gym.Domain._Common;

namespace Gym.Domain.TrainingContext.Errors
{
    public class TrainingNameValidationError : DomainError
    {
        private TrainingNameValidationError() : base(nameof(TrainingNameValidationError)) { }

        public static TrainingNameValidationError Create() => new();

        public override String GetErrorMessage() => $"Training name is invalid.";
    }
}
