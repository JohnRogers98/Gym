using Gym.Domain._Common;
using Gym.Domain.ClientContext.ValueObjects;

namespace Gym.Domain.ClientContext.Errors
{
    public class ClientNotFoundError : DomainError
    {
        public ClientId ClientId { get; }

        private ClientNotFoundError(ClientId clientId) : base(nameof(ClientNotFoundError))
        {
            ClientId = clientId;
        }

        public static ClientNotFoundError Create(ClientId clientId) => new(clientId);

        public override String GetErrorMessage() => $"Client with id - {ClientId.Value} not found.";
    }
}
