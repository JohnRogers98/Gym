namespace Gym.WebDto.Responses.Users
{
    public record UserDto
    {
        public required String Id { get; init; }

        public String? FirstName { get; init; }

        public String? LastName { get; init; }

        public required String RoleId { get; init; }
    }
}
