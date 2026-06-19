namespace Gym.WebDto.Requests.Roles
{
    public record CreateUserRoleRequest
    {
        public required String Name { get; init; }
    }
}
