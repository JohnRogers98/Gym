using Gym.Domain._Shared.Services;
using Gym.Domain.PollContext;
using Gym.Domain.PollContext.ValueObjects;
using Gym.Domain.PollResponseContext;
using Gym.Domain.PollResponseContext.Errors;
using Gym.Domain.PollResponseContext.Events;

namespace Gym.Domain.Tests.Services
{
    public class SubmitPollResponseServiceTests
    {
        private FakeDataFixture _fakeDataFixture = new FakeDataFixture();

        [Fact]
        public void Submit_Poll_Response_To_Poll()
        {
            List<ChoiceText> choices = [_fakeDataFixture.CreateChoiceText("Variant_1"), _fakeDataFixture.CreateChoiceText("Variant_2")];
            Poll poll = _fakeDataFixture.CreatePoll(pollId: _fakeDataFixture.PollId.Value, isResponseRequired: true, canAcceptManyChoices: true, choices: choices);
            var userId = _fakeDataFixture.GenerateUserId();
            PollResponse pollResponse = PollResponse.Create(userId, _fakeDataFixture.PollId, [_fakeDataFixture.CreateChoiceId(1)]);
            SubmitPollResponseService sut = new();

            var result = sut.Submit(poll: poll, pollResponse: pollResponse);

            Assert.True(result.Success);
            Assert.NotEqual(default, pollResponse.DomainEvents.OfType<CalendarEventPollResponseCreatedDomainEvent>().SingleOrDefault());
        }

        [Fact]
        public void Error_When_Validate_Poll_Response_With_Poll_Id_Which_Is_Not_The_Same_As_Poll()
        {
            List<ChoiceText> choices = [_fakeDataFixture.CreateChoiceText("Variant_1"), _fakeDataFixture.CreateChoiceText("Variant_2")];
            Poll poll = _fakeDataFixture.CreatePoll(pollId: _fakeDataFixture.GeneratePollId().Value, isResponseRequired: true, canAcceptManyChoices: true, choices: choices);
            var userId = _fakeDataFixture.GenerateUserId();
            PollResponse pollResponse = PollResponse.Create(userId, _fakeDataFixture.GeneratePollId(), [_fakeDataFixture.CreateChoiceId(1)]);
            SubmitPollResponseService sut = new();

            var result = sut.Submit(poll: poll, pollResponse: pollResponse);

            Assert.IsType<PollResponseIsNotConsistentWithTemplateError>(result.Error);
        }

        [Fact]
        public void Error_When_Validate_Poll_Response_With_Not_The_Same_Choices_As_In_Poll()
        {
            List<ChoiceText> choices = [_fakeDataFixture.CreateChoiceText("Variant_1"), _fakeDataFixture.CreateChoiceText("Variant_2")];
            Poll poll = _fakeDataFixture.CreatePoll(pollId: _fakeDataFixture.PollId.Value, isResponseRequired: true, canAcceptManyChoices: true, choices: choices);
            var userId = _fakeDataFixture.GenerateUserId();
            PollResponse pollResponse = PollResponse.Create(userId, _fakeDataFixture.PollId, [_fakeDataFixture.CreateChoiceId(3)]);
            SubmitPollResponseService sut = new();

            var result = sut.Submit(poll: poll, pollResponse: pollResponse);

            Assert.IsType<PollResponseIsNotConsistentWithTemplateError>(result.Error);
        }

        [Fact]
        public void Error_When_Validate_Poll_Response_With_Many_Choices_When_Poll_Restrict_It_To_One()
        {
            List<ChoiceText> choices = [_fakeDataFixture.CreateChoiceText("Variant_1"), _fakeDataFixture.CreateChoiceText("Variant_2")];
            Poll poll = _fakeDataFixture.CreatePoll(pollId: _fakeDataFixture.PollId.Value, isResponseRequired: true, canAcceptManyChoices: false, choices: choices);
            var userId = _fakeDataFixture.GenerateUserId();
            PollResponse pollResponse = PollResponse.Create(userId, _fakeDataFixture.PollId, [_fakeDataFixture.CreateChoiceId(1), _fakeDataFixture.CreateChoiceId(2)]);
            SubmitPollResponseService sut = new();

            var result = sut.Submit(poll: poll, pollResponse: pollResponse);

            Assert.IsType<PollResponseIsNotConsistentWithTemplateError>(result.Error);
        }
    }
}
