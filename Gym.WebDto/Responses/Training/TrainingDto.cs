namespace Gym.WebDto.Responses.Training
{
    public record TrainingDto
    {
        public required String Id { get; init; }
        public required String Name { get; init; }
        public String? Description { get; init; }
    }
}
