using Gym.Domain._Common;

namespace Gym.Domain.UserContext.Errors
{
    public class UserRoleParseError : DomainError
    {
        private UserRoleParseError() : base(nameof(UserRoleParseError)) { }

        public static UserRoleParseError Create() => new();

        public override String GetErrorMessage() => $"User role cannot be parsed.";
    }
}
