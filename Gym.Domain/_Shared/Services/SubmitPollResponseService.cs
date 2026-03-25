using Gym.Application.Extensions;
using Gym.Domain._Common;
using Gym.Domain.PollContext;
using Gym.Domain.PollResponseContext;
using Gym.Domain.PollResponseContext.Errors;

namespace Gym.Domain._Shared.Services
{
    public interface ISubmitPollResponseService
    {
        Result Submit(Poll poll, PollResponse pollResponse);
    }

    public class SubmitPollResponseService : ISubmitPollResponseService
    {
        public Result Submit(Poll poll, PollResponse pollResponse)
        {
            return this.ValidatePollResponse(poll, pollResponse);
        }

        private Result ValidatePollResponse(Poll poll, PollResponse pollResponse)
        {
            if (poll!.Id != pollResponse.PollId)
                return Result.Fail(PollResponseIsNotConsistentWithTemplateError.Create(poll.Id, pollResponse));

            foreach (var aResponseChoice in pollResponse.Choices)
            {
                if (poll.Choices.Select(aPollChoice => aPollChoice.Id).Contains(aResponseChoice) is false)
                {
                    return Result.Fail(PollResponseIsNotConsistentWithTemplateError.Create(poll.Id, pollResponse));
                }
            }

            if (poll.CanAcceptManyChoices.Value is false && pollResponse.Choices.Count > 1)
            {
                return Result.Fail(PollResponseIsNotConsistentWithTemplateError.Create(poll.Id, pollResponse));
            }

            return Result.Ok();
        }
    }

}
