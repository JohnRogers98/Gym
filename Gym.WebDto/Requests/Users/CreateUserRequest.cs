namespace Gym.WebDto.Requests.Users
{
    public record CreateUserRequest
    {
        public required String Login { get; init; }
        public required String Role { get; init; }
        public required String FirstName { get; init; }
        public String? LastName { get; init; }
    }
}
