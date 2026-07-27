using Gym.Domain._Common;
using Gym.Domain.PersonalTrainingContext.ValueObjects;

namespace Gym.Domain.PersonalTrainingContext.Errors
{
    public class PersonalTrainingNotFoundError : DomainError
    {
        public PersonalTrainingId PersonalTrainingId { get; }

        private PersonalTrainingNotFoundError(PersonalTrainingId personalTrainingId) : base(nameof(PersonalTrainingNotFoundError))
        {
            PersonalTrainingId = personalTrainingId;
        }

        public static PersonalTrainingNotFoundError Create(PersonalTrainingId personalTrainingId) => new(personalTrainingId);

        public override String GetErrorMessage() => $"Personal training with id - {PersonalTrainingId.Value} not found.";
    }
}
