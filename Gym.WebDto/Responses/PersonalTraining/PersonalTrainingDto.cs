namespace Gym.WebDto.Responses.PersonalTraining
{
    public record PersonalTrainingDto
    {
        public required String Id { get; init; }
        public required InstructorInfo Intructor { get; init; }
        public required ClientInfo Client { get; init; }
        public required String Status { get; init; }
        public required DateTime Start { get; init; }
        public required DateTime? End { get; init; }
        public required String PaymentStatus { get; init; }
        public required String InstructorComment { get; init; }
        public required String ClientComment { get; init; } 
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
