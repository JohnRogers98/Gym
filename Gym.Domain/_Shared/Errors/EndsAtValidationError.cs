using Gym.Domain._Common;

namespace Gym.Domain._Shared.Errors
{
    public class EndsAtValidationError : DomainError
    {
        private EndsAtValidationError() : base(nameof(EndsAtValidationError)) { }

        public static EndsAtValidationError Create() => new();

        public override String GetErrorMessage() => $"Ends at is invalid.";
    }
}
