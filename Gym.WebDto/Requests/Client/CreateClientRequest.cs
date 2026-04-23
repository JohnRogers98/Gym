namespace Gym.WebDto.Requests.Client
{
    public record CreateClientRequest
    {
        public required String Login { get; init; }
        public required String FirstName { get; init; }
        public String? LastName { get; init; }
    }
}
