namespace Gym.WebDto.Responses.Users
{
    public record CheckUsernameExistenceResponse
    {
        public required Boolean IsExist { get; init; }
    }
}
