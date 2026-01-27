namespace Gym.WebDto.Responses.Instructor
{
    public record CreateInstructorResponse
    {
        public required String Id { get; init; }
        public required String FirstName { get; init; }
        public String? LastName { get; init; }
    }
}
