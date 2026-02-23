namespace Gym.WebDto.Requests.Training
{
    public record CreateTrainingRequest 
    {
        public required String Name { get; init; }
        public String? Description { get; init; }
    }
}
