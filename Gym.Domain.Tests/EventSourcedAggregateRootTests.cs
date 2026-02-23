using Gym.Domain._Common;

namespace Gym.Domain.Tests
{
    public class EventSourcedAggregateRootTests
    {
        [Fact]
        public void Apply_Event_Dynamic_Dispatch_Choose_Correct_Method_By_Convention()
        {
            MockedEventSourcedAggregate sut = new();
            MockedDomainEvent mockedEvent = new();

            sut.ApplyEvent(mockedEvent);

            Assert.True(sut.WasCorrectMethodCalled);
        }

        public class MockedDomainEvent : DomainEvent;

        public class MockedEventSourcedAggregate : EventSourcedAggregateRoot
        {
            public Boolean WasCorrectMethodCalled { get; set; } = false;

            public void ApplyEvent(MockedDomainEvent mockedEvent) => WasCorrectMethodCalled = true;
        }

    }
}
