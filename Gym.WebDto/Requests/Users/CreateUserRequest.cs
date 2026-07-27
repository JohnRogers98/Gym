namespace Gym.WebDto.Requests.Users
{
    public record CreateUserRequest
    {
        public required String Username { get; init; }
        public required String Password { get; init; }
        public required String FirstName { get; init; }
        public String? LastName { get; init; }
        public required String RoleId  { get; init; }
    }
}
