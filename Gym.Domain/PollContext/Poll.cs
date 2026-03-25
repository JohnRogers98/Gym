using Gym.Application.Extensions;
using Gym.Domain._Common;
using Gym.Domain.PollContext.Errors;
using Gym.Domain.PollContext.ValueObjects;

namespace Gym.Domain.PollContext
{
    public class Poll : AggregateRoot
    {
        public PollId Id { get; }

        public PollTitle Title { get; }

        public IsResponseRequired IsResponseRequired { get; }

        public CanAcceptManyChoices CanAcceptManyChoices { get; }

        public IReadOnlyCollection<Choice> Choices { get; }

        
        private Poll(PollId id, PollTitle title, IsResponseRequired isResponseRequired, CanAcceptManyChoices canAcceptManyChoices, IReadOnlyCollection<Choice> choices)
        {
            (Id, Title, IsResponseRequired, CanAcceptManyChoices, Choices) = (id, title, isResponseRequired, canAcceptManyChoices, choices);
        }

        public static Result<Poll> Create(
            PollId id,
            PollTitle title,
            IsResponseRequired isResponseRequired,
            CanAcceptManyChoices canAcceptManyChoices,
            IReadOnlyCollection<ChoiceText> choiceTexts)
        {
            if(choiceTexts.Any() is false)
            {
                return Result<Poll>.Fail(PollHasNoChoicesError.Create());
            }

            var duplicateTexts = choiceTexts
             .GroupBy(aChoice => aChoice)
             .Where(group => group.Count() > 1)
             .Select(group => group.Key)
             .ToList();

            if (duplicateTexts.Any())
            {
                return Result<Poll>.Fail(PollHasDuplicatedChoiceTextError.Create(duplicateTexts));
            }

            return Result<Poll>.Ok(new(id, title, isResponseRequired, canAcceptManyChoices, CreateChoices(choiceTexts)));
        }
        private static IReadOnlyCollection<Choice> CreateChoices(IReadOnlyCollection<ChoiceText> choiceTexts)
        {
            Int32 idCounter = 1;

            return choiceTexts.Select(aChoiceText => 
            {
                var choiceId = ChoiceId.From(idCounter);
                idCounter++;

                return Choice.From(choiceId.Unwrap(), aChoiceText);
            }).ToList();
        }

        public static Poll Restore(
          PollId id,
          PollTitle title,
          IsResponseRequired isResponseRequired,
          CanAcceptManyChoices canAcceptManyChoices,
          IReadOnlyCollection<Choice> choices)
        {
            return new(id, title, isResponseRequired, canAcceptManyChoices, choices);
        }

    }
}
