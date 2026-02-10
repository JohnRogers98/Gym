using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.ClientContext.Events;

namespace Gym.Domain.ClientContext
{
    public class Client : AggregateRoot
    {
        public ClientId Id { get; }
        public UserId UserId { get; }

        private Client(ClientId id, UserId userId)
        {
            Id = id;
            UserId = userId;
        }

        public static Client Create(ClientId id, UserId userId)
        {
            Client client = new(id, userId);
            client.AddDomainEvent(CreatedNewClientDomainEvent.Create(client.UserId));
            return client;
        }

        public static Client Restore(ClientId id, UserId userId)
            => new(id, userId);

        public override String ToString()
            => $"{nameof(Id)}: {Id} \t {nameof(UserId)}: {UserId}";

        public override Boolean Equals(Object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is Client other && Id == other.Id;
        }

        public override Int32 GetHashCode() => Id.GetHashCode();
    }
}
