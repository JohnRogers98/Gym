using Gym.Application.Extensions;
using Gym.Domain.PollContext;
using Gym.Domain.PollContext.ValueObjects;
using Gym.Infrastructure.Entities.Repositories.Polls;

namespace Gym.Infrastructure.Entities.Extensions.Mappings
{
    internal static class PollExtensions
    {
        public static Poll ToDomain(this PollEntity pollEntity)
        {
            return Poll.Restore(
                PollId.From(pollEntity.Id.ToString()).Unwrap(),
                PollTitle.From(pollEntity.Title).Unwrap(),
                IsResponseRequired.From(pollEntity.IsRequired),
                CanAcceptManyChoices.From(pollEntity.CanAcceptManyChoices),
                [.. pollEntity.Choices.Select(record => record.ToDomain())]
            );
        }

        public static Choice ToDomain(this ChoiceRecord choiceRecord)
        {
            return Choice.From(
                ChoiceId.From(choiceRecord.Id).Unwrap(),
                ChoiceText.From(choiceRecord.Text).Unwrap()
            );
        }

        public static PollEntity ToEntity(this Poll poll)
        {
            return new PollEntity
            {
                Id = poll.Id.Value.ToObjectId(),
                Title = poll.Title.Value,
                IsRequired = poll.IsResponseRequired.Value,
                CanAcceptManyChoices = poll.CanAcceptManyChoices.Value,
                Choices = [.. poll.Choices.Select(aChoice => aChoice.ToRecord())]
            };
        }

        public static ChoiceRecord ToRecord(this Choice choice)
        {
            return new ChoiceRecord
            {
                Id = choice.Id.Value,
                Text = choice.Text.Value
            };
        }
    }
}
