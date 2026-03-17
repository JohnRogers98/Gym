using Gym.Domain._Common;

namespace Gym.Domain.AccountContext.Errors
{
    public class RemainingTrainingsValidationError : DomainError
    {
        private RemainingTrainingsValidationError() : base(nameof(RemainingTrainingsValidationError)) { }

        public static RemainingTrainingsValidationError Create() => new();

        public override String GetErrorMessage() => $"Remaining trainings is invalid.";
    }
}
