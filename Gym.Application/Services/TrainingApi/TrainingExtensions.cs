using Gym.Domain.TrainingContext;

namespace Gym.Application.Services.TrainingApi
{
    internal static class TrainingExtensions
    {
        public static TrainingDetails ToDetails(this Training training) 
            => new TrainingDetails(training.Id.Value, training.Name, training.Description);
    }
}
