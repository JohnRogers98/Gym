using Gym.WebApplication.Features.Admin.Shared.ValueObjects;

namespace Gym.WebApplication.Features.Admin.Shared.Models
{
    public class GetTrainingById
    {
        public required TrainingId TrainingId { get; set; }
    }
}
