using Gym.Domain._Common;

namespace Gym.Domain.PollContext.Errors
{
    public class PollTitleValidationError : DomainError
    {
        private PollTitleValidationError() : base(nameof(PollTitleValidationError)) { }

        public static PollTitleValidationError Create() => new();

        public override String GetErrorMessage() => $"Poll title is invalid.";
    }
}
