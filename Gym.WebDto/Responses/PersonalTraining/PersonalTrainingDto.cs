namespace Gym.WebDto.Responses.PersonalTraining
{
    public record PersonalTrainingDto
    {
        public required String Id { get; init; }
        public required InstructorInfo Instructor { get; init; }
        public required ClientInfo Client { get; init; }
        public required String Status { get; init; }
        public required DateTime Start { get; init; }
        public DateTime? End { get; init; }
        public required String PaymentStatus { get; init; }
        public String? InstructorComment { get; init; }
        public String? ClientComment { get; init; } 
    }

    public record InstructorInfo
    {
        public required String Id { get; init; }
        public required String FullName { get; init; }
    }

    public record ClientInfo
    {
        public required String Id { get; init; }
        public required String FullName { get; init; }
    }
}
