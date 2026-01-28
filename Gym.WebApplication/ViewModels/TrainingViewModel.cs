namespace Gym.WebApplication.ViewModels
{
    public record TrainingViewModel
    {
        public required String Id { get; init; }
        public required String Name { get; init; }
        public String? Description { get; init; }
    }
}
