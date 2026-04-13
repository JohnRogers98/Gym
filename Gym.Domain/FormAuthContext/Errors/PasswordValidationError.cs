using Gym.Domain._Common;

namespace Gym.Domain.FormAuthContext.Errors
{
    public class PasswordValidationError : DomainError
    {
        private PasswordValidationError() : base(nameof(PasswordValidationError)) { }

        public static PasswordValidationError Create() => new();

        public override String GetErrorMessage() => $"Password is invalid.";
    }
}
