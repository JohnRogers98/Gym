using Gym.Domain._Common;

namespace Gym.Domain.CalendarEventContext.Errors
{
    public class CapacityValidationError : DomainError
    {
        private CapacityValidationError() : base(nameof(CapacityValidationError)) { }

        public static CapacityValidationError Create() => new();

        public override String GetErrorMessage() => $"Capacity is invalid.";
    }
}
