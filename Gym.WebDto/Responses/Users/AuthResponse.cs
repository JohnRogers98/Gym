namespace Gym.WebDto.Responses.Users
{
    public record AuthResponse
    {
        public required String UserId { get; init; }
        public required String Role { get; init; }
    }
}
