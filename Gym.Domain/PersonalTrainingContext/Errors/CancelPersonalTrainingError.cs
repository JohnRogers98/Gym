using Gym.Domain._Common;
using Gym.Domain.PersonalTrainingContext.ValueObjects;

namespace Gym.Domain.PersonalTrainingContext.Errors
{
    public class CancelPersonalTrainingError : DomainError
    {
        public PersonalTrainingId PersonalTrainingId { get; }

        private CancelPersonalTrainingError(PersonalTrainingId personalTrainingId) : base(nameof(CancelPersonalTrainingError))
        {
            PersonalTrainingId = personalTrainingId;
        }

        public static CancelPersonalTrainingError Create(PersonalTrainingId personalTrainingId) => new(personalTrainingId);

        public override String GetErrorMessage() => $"Personal training with id - {PersonalTrainingId.Value} cannot be cancelled.";
    }
}
