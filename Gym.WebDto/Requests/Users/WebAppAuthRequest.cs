namespace Gym.WebDto.Requests.Users
{
    public record WebAppAuthRequest
    {
        public required String InitData { get; init; }
    }
}
