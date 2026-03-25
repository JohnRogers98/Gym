using Gym.Application.Extensions;
using Gym.Domain._Shared;
using Gym.Domain.PollContext.ValueObjects;
using Gym.Domain.PollResponseContext;
using Gym.Infrastructure.Entities.Repositories.PollResponses;

namespace Gym.Infrastructure.Entities.Extensions.Mappings
{
    internal static class PollResponseExtensions
    {
        public static PollResponse ToDomain(this PollResponseEntity pollResponseEntity)
        {
            return PollResponse.Restore(
                UserId.From(pollResponseEntity.UserId.ToString()).Unwrap(),
                PollId.From(pollResponseEntity.PollId.ToString()).Unwrap(),
                [.. pollResponseEntity.ChoiceIds.Select(choiceId => ChoiceId.From(choiceId).Unwrap())]
            );
        }

        public static PollResponseEntity ToEntity(this PollResponse pollResponse)
        {
            return new PollResponseEntity
            {
                Id = pollResponse.Id.Value,
                UserId = pollResponse.UserId.Value.ToObjectId(),
                PollId = pollResponse.PollId.Value.ToObjectId(),
                ChoiceIds = [.. pollResponse.Choices.Select(choiceId => choiceId.Value)]
            };
        }
    }
}
