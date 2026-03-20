using Gym.Domain._Common;
using Gym.Domain.PollContext.ValueObjects;

namespace Gym.Domain.PollContext.Errors
{
    public class PollHasDuplicatedChoiceTextError : DomainError
    {
        public IReadOnlyCollection<ChoiceText> ChoiceTexts { get; }

        private PollHasDuplicatedChoiceTextError(IReadOnlyCollection<ChoiceText> choiceTexts) : base(nameof(PollHasDuplicatedChoiceTextError))
        {
            ChoiceTexts = choiceTexts;
        }

        public static PollHasDuplicatedChoiceTextError Create(IReadOnlyCollection<ChoiceText> choiceTexts) => new(choiceTexts);

        public override String GetErrorMessage() => $"Choice texts - [{String.Join(", ", ChoiceTexts)}] are duplicated.";
    }
}
