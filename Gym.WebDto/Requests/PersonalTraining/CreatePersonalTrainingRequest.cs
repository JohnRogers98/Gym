namespace Gym.WebDto.Requests.PersonalTraining
{
    public record CreatePersonalTrainingRequest
    {
        public required String ClientId { get; init; }
        public required DateTime Start { get; init; }
        public required DateTime? End { get; init; }
        public required Boolean IsPaid { get; init; }
        public required String InstructorComment { get; init; }
    }
}
