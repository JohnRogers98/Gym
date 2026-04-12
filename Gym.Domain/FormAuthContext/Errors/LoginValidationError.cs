using Gym.Domain._Common;

namespace Gym.Domain.FormAuthContext.Errors
{
    public class LoginValidationError : DomainError
    {
        private LoginValidationError() : base(nameof(LoginValidationError)) { }

        public static LoginValidationError Create() => new();

        public override String GetErrorMessage() => $"Login is invalid.";
    }
}
