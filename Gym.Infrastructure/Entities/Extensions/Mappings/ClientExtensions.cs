using Gym.Application.Extensions;
using Gym.Domain._Shared;
using Gym.Domain.ClientContext;
using Gym.Domain.ClientContext.ValueObjects;
using Gym.Infrastructure.Entities.Repositories.Clients;

namespace Gym.Infrastructure.Entities.Extensions.Mappings
{
    internal static class ClientExtensions
    {
        public static Client ToDomain(this ClientEntity entity)
        {
            return Client.Restore(
                ClientId.From(entity.Id.ToString()).Unwrap(),
                UserId.From(entity.UserId.ToString()).Unwrap()
            );
        }

        public static ClientEntity ToEntity(this Client client)
        {
            return new() 
            { 
                Id = client.Id.Value.ToObjectId(),
                UserId = client.UserId.Value.ToObjectId()
            };
        }
    }
}
