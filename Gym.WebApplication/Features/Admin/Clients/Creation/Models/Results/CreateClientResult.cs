namespace Gym.WebApplication.Features.Admin.Clients.Creation.Models.Results
{
    public record CreateClientResult
    {
        public required String UserId { get; init; }
        public required String Login { get; init; }
        public required String Password { get; init; }
    }
}
