namespace Gym.WebDto.Responses.Training
{
    public record GetTrainingResponse
    {
        public required String Id { get; init; }
        public required String Name { get; init; }
        public String? Description { get; init; }
    }
}
