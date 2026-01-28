namespace Gym.WebDto.Requests.Instructor
{
    public record CreateInstructorRequest
    {
        public required String FirstName { get; init; }
        public required String LastName { get; init; }
    }
}
