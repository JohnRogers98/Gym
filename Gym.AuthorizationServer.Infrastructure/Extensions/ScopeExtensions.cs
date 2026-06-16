using Gym.AuthorizationServer.Infrastructure.Entities.UserConsents;

namespace Gym.AuthorizationServer.Infrastructure.Entities.Scopes;

public static class ScopeExtensions
{
    public static ScopeInfo ToInfo(this ScopeEntity entity) 
        => new ScopeInfo() { Id = entity.Id, Name = entity.Name };
}
