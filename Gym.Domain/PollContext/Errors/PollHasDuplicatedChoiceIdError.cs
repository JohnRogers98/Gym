using Gym.Domain._Common;
using Gym.Domain.PollContext.ValueObjects;

namespace Gym.Domain.PollContext.Errors
{
    public class PollHasDuplicatedChoiceIdError : DomainError
    {
        public IReadOnlyCollection<ChoiceId> ChoiceIds { get; }

        private PollHasDuplicatedChoiceIdError(IReadOnlyCollection<ChoiceId> choiceIds) : base(nameof(PollHasDuplicatedChoiceIdError)) 
        {
            ChoiceIds = choiceIds;
        }

        public static PollHasDuplicatedChoiceIdError Create(IReadOnlyCollection<ChoiceId> choiceIds) => new(choiceIds);

        public override String GetErrorMessage() => $"Choice ids - [{String.Join(", ", ChoiceIds.Select(choiceId => choiceId.Value))}] are duplicated.";

    }
}
