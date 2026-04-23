namespace Gym.WebDto.Responses.Clients
{
    public record CreateClientResponse
    {
        public required String ClientId { get; init; }
        public required String Login { get; init; }
        public required String Password { get; init; }
    }
}
