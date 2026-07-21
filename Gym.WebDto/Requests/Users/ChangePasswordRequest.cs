namespace Gym.WebDto.Requests.Users
{
    public record ChangePasswordRequest
    {
        public required String CurrentPassword { get; init; }
        public required String NewPassword { get; init; }
    }
}
