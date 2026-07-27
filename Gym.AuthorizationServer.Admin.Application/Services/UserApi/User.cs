namespace Gym.AuthorizationServer.Admin.Application.Services.UserApi
{
    public record User
    {
        public required String Id { get; init; }

        public String? FirstName { get; init; }

        public String? LastName { get; init; }

        public required String RoleId { get; init; }
    }
}
