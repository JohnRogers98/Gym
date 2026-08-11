namespace Gym.AuthorizationServer.Admin.Application.Services.RoleApi
{
    public record UserRole()
    {
        public required String Id { get; init; }
        public required String Name { get; init; }
    }
}
