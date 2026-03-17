namespace Gym.Domain._Common
{
    public abstract class EventSourcedAggregateRoot : AggregateRoot
    {
        public void ApplyEvent(DomainEvent @event)
        {
            ((dynamic)this).ApplyEvent((dynamic)@event);
        }
    }
}
