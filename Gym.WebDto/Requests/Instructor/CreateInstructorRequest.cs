namespace Gym.WebDto.Requests.Instructor
{
    public record CreateInstructorRequest
    {
        public required String Login { get; init; }
        public required String FirstName { get; init; }
        public String? LastName { get; init; }
    }
}
