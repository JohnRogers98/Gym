using Gym.Domain._Common;

namespace Gym.Domain.FormAuthContext.Errors
{
    public class LoginAlreadyExistsError : DomainError
    {
        private LoginAlreadyExistsError() : base(nameof(LoginAlreadyExistsError)) { }

        public static LoginAlreadyExistsError Create() => new();

        public override String GetErrorMessage() => $"Login already exists.";
    }
}
