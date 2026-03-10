namespace Gym.WebDto.Responses.Instructor
{
    public record InstructorDto
    {
        public required String Id { get; init; }
        public required String FullName { get; init; }
    }
}
