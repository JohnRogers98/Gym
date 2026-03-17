using Gym.Domain._Common;

namespace Gym.Domain._Shared.Errors
{
    public class LastNameValidationError : DomainError
    {
        private LastNameValidationError() : base(nameof(LastNameValidationError)) { }

        public static LastNameValidationError Create() => new();

        public override String GetErrorMessage() => $"Last name is invalid.";
    }
}
