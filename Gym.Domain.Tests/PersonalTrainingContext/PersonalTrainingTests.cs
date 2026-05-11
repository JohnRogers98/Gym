using Gym.Application.Extensions;
using Gym.Domain.PersonalTrainingContext;
using Gym.Domain.PersonalTrainingContext.Errors;
using Gym.Domain.PersonalTrainingContext.Events;

namespace Gym.Domain.Tests.PersonalTrainingContext
{
    public class PersonalTrainingTests
    {
        private readonly FakeDataFixture _fakeDataFixture = new();

        [Fact]
        public void Check_Training_Cancellation()
        {
            PersonalTraining sut = _fakeDataFixture.CreatePersonalTraining();

            var result = sut.Cancel();

            Assert.True(result.Success);
            Assert.NotEqual(default, sut.DomainEvents.OfType<PersonalTrainingCancelledDomainEvent>().SingleOrDefault());
        }

        [Fact]
        public void Error_When_Cancel_With_Invalid_Status()
        {
            PersonalTraining sut = _fakeDataFixture.CreatePersonalTraining();

            var result = sut.Cancel()
                .Bind(sut.Cancel);

            Assert.False(result.Success);
            Assert.IsType<CancelPersonalTrainingError>(result.Error);
        }
    }
}
