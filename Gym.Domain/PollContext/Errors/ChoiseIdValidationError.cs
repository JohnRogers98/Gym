using Gym.Domain._Common;

namespace Gym.Domain.PollContext.Errors
{
    public class ChoiseIdValidationError : DomainError
    {
        private ChoiseIdValidationError() : base(nameof(ChoiseIdValidationError)) { }

        public static ChoiseIdValidationError Create() => new();

        public override String GetErrorMessage() => $"Choice id is invalid.";
    }
}
