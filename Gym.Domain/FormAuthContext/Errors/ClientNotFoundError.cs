using Gym.Domain._Common;
using Gym.Domain.FormAuthContext.ValueObjects;

namespace Gym.Domain.ClientContext.Errors
{
    public class SuchLoginNotExistsError : DomainError
    {
        public Login Login { get; }

        private SuchLoginNotExistsError(Login login) : base(nameof(SuchLoginNotExistsError))
        {
            Login = login;
        }

        public static SuchLoginNotExistsError Create(Login login) => new(login);

        public override String GetErrorMessage() => $"Login - {Login.Value} not found.";
    }
}
