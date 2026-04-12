namespace Gym.WebDto.Responses.Users
{
    public record CreateUserResponse
    {
        public required String UserId { get; init; }
        public required String Login { get; init; }
        public required String Password { get; init; }
    }
}
