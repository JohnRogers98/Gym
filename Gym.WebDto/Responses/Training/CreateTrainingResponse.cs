namespace Gym.WebDto.Responses.Training
{
    public record CreateTrainingResponse
    {
        public required String Id { get; init; }
        public required String Name { get; init; }
        public String? Description { get; init; }
    }
}
