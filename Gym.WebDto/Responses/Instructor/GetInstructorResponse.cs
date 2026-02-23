namespace Gym.WebDto.Responses.Instructor
{
    public record GetInstructorResponse
    {
        public required String Id { get; init; }
        public required String FullName { get; init; }
    }
}
