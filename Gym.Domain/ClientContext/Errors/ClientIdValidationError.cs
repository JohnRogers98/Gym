using Gym.Domain._Common;

namespace Gym.Domain.ClientContext.Errors
{
    public class ClientIdValidationError : DomainError
    {
        private ClientIdValidationError() : base(nameof(ClientIdValidationError)) { }

        public static ClientIdValidationError Create() => new();

        public override String GetErrorMessage() => $"Client id is invalid.";
    }
}
