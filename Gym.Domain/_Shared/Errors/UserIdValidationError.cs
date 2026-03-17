using Gym.Domain._Common;

namespace Gym.Domain._Shared.Errors
{
    public class UserIdValidationError : DomainError
    {
        private UserIdValidationError() : base(nameof(UserIdValidationError)) { }

        public static UserIdValidationError Create() => new();

        public override String GetErrorMessage() => $"User id is invalid.";
    }
}
