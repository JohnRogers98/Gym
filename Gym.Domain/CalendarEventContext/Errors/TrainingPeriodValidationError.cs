using Gym.Domain._Common;

namespace Gym.Domain.CalendarEventContext.Errors
{
    public class TrainingPeriodValidationError : DomainError
    {
        private TrainingPeriodValidationError() : base(nameof(TrainingPeriodValidationError)) { }

        public static TrainingPeriodValidationError Create() => new();

        public override String GetErrorMessage() => $"Training period is invalid.";
    }
}
