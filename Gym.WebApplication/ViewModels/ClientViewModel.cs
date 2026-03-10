namespace Gym.WebApplication.ViewModels
{
    public record ClientViewModel
    {
        public required String Id { get; init; }
        public String? Username { get; init; }
        public String? FirstName { get; init; }
        public String? LastName { get; init; }
        public required Int32 AvailableTrainingsCount { get; init; }

        public String FullName => $"{FirstName ?? String.Empty} {LastName ?? String.Empty}";  
    }
}
