using Gym.Domain._Common;

namespace Gym.Domain.PollContext.Errors
{
    public class PollIdValidationError : DomainError
    {
        private PollIdValidationError() : base(nameof(PollIdValidationError)) { }

        public static PollIdValidationError Create() => new();

        public override String GetErrorMessage() => $"Poll id is invalid.";
    }
}
