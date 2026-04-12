using Gym.Domain._Common;

namespace Gym.Domain.FormAuthContext.Errors
{
    public class PasswordHashMatchError : DomainError
    {
        private PasswordHashMatchError() : base(nameof(PasswordHashMatchError)) { }

        public static PasswordHashMatchError Create() => new();

        public override String GetErrorMessage() => $"Password hash is invalid.";
    }
}
