namespace Gym.WebDto.Responses.Users
{
    public record WebAppAuthResponse
    {
        public required String Id { get; init; }
        public required String Role { get; init; }
    }
}
