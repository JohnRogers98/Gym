namespace Gym.WebDto.Requests.Account
{
    public record ChargeAccountRequest
    {
        public required Int32 ByCount { get; init; }
    }
}
