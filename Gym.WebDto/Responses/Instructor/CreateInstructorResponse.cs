namespace Gym.WebDto.Responses.Instructor
{
    public record CreateInstructorResponse
    {
        public required String InstructorId { get; init; }
        public required String Login { get; init; }
        public required String Password { get; init; }
    }
}
