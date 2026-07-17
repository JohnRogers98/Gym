namespace Gym.WebDto.Requests.Client
{
    public record CreateClientRequest
    {
        public required String UserId { get; set; }
        public required String FirstName { get; init; }
        public String? LastName { get; init; }
    }
}
