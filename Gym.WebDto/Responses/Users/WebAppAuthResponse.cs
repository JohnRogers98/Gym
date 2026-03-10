namespace Gym.WebDto.Responses.Users
{
    public record WebAppAuthResponse
    {
        public required String UserId { get; init; }
        public required String Role { get; init; }
    }
}
