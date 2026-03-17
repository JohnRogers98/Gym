using Gym.Domain._Common;
using Gym.Domain.TrainingContext.ValueObjects;

namespace Gym.Domain.TrainingContext.Errors
{
    public class TrainingNotFoundError : DomainError
    {
        public TrainingId TrainingId { get; }

        private TrainingNotFoundError(TrainingId trainingId) : base(nameof(TrainingNotFoundError)) 
        {
            TrainingId = trainingId;
        }

        public static TrainingNotFoundError Create(TrainingId trainingId) => new(trainingId);

        public override String GetErrorMessage() => $"Training with id - {TrainingId.Value} not found.";
    }
}
