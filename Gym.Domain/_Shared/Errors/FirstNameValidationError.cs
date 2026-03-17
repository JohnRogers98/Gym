using Gym.Domain._Common;

namespace Gym.Domain._Shared.Errors
{
    public class FirstNameValidationError : DomainError
    {
        private FirstNameValidationError() : base(nameof(FirstNameValidationError)) { }

        public static FirstNameValidationError Create() => new();

        public override String GetErrorMessage() => $"First name is invalid.";
    }
}
