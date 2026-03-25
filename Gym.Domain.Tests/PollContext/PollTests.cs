using Gym.Application.Extensions;
using Gym.Domain.PollContext;
using Gym.Domain.PollContext.Errors;
using Gym.Domain.PollContext.ValueObjects;

namespace Gym.Domain.Tests.PollContext
{
    public class PollTests
    {
        private readonly FakeDataFixture _fakeDataFixture = new();

        [Fact]
        public void Check_New_That_Poll_Generate_Correct_Ids_For_Choices()
        {
            var sut = Poll.Create(
                _fakeDataFixture.GeneratePollId(),
                _fakeDataFixture.CreatePollTitle("title"),
                IsResponseRequired.From(true),
                CanAcceptManyChoices.From(true),
                [ChoiceText.From("variant_1").Unwrap(), ChoiceText.From("variant_2").Unwrap()]
            ).Unwrap();

            Assert.Equal(2, sut.Choices.Count);
            Assert.Equal(1, sut.Choices.ElementAt(0).Id.Value);
            Assert.Equal(2, sut.Choices.ElementAt(1).Id.Value);
        }

        [Fact]
        public void Returns_Error_When_Choice_Texts_Are_The_Same()
        {
            var createSutResult = Poll.Create(
                _fakeDataFixture.GeneratePollId(),
                _fakeDataFixture.CreatePollTitle("title"),
                IsResponseRequired.From(true),
                CanAcceptManyChoices.From(true),
                [ChoiceText.From("variant_1").Unwrap(), ChoiceText.From("variant_1").Unwrap()]
            );

            Assert.IsType<PollHasDuplicatedChoiceTextError>(createSutResult.Error);
        }
    }
}
