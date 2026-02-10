namespace Gym.WebDto.Requests.Account
{
    public record ChargeAccountRequest
    {
        public required String ClientId { get; init; }
        public required Int32 ByCount { get; init; }
    }
}
