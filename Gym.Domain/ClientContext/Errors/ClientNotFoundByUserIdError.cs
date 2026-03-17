using Gym.Domain._Common;
using Gym.Domain._Shared;

namespace Gym.Domain.ClientContext.Errors
{
    public class ClientNotFoundByUserIdError : DomainError
    {
        public UserId UserId { get; }

        private ClientNotFoundByUserIdError(UserId userId) : base(nameof(ClientNotFoundByUserIdError))
        {
            UserId = userId;
        }

        public static ClientNotFoundByUserIdError Create(UserId userId) => new(userId);

        public override String GetErrorMessage() => $"Client with user id - {UserId.Value} not found.";
    }
}
