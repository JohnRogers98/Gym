using Gym.Domain._Common;

namespace Gym.Domain.PollContext.Errors
{
    public class ChoiceTextValidationError : DomainError
    {
        private ChoiceTextValidationError() : base(nameof(ChoiceTextValidationError)) { }

        public static ChoiceTextValidationError Create() => new();

        public override String GetErrorMessage() => $"Choice text is invalid.";
    }
}
