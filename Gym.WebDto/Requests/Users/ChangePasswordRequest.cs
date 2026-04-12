namespace Gym.WebDto.Requests.Users
{
    public record ChangePasswordRequest
    {
        public required String OldPassword { get; init; }
        public required String NewPassword { get; init; }
    }
}
