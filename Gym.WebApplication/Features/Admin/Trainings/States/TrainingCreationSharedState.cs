using Gym.WebApplication.Features.Admin.Trainings.Creation.Models.Results;

namespace Gym.WebApplication.Features.Admin.Trainings.States
{
    public interface ITrainingCreationSharedState
    {
        event Action<CreateTrainingResult>? TrainingCreated;

        void Notify(CreateTrainingResult createTrainingResult);
    }

    public class TrainingCreationSharedState : ITrainingCreationSharedState
    {
        public event Action<CreateTrainingResult>? TrainingCreated;

        public void Notify(CreateTrainingResult createTrainingResults)
        {
            TrainingCreated?.Invoke(createTrainingResults);
        }
    }
}
