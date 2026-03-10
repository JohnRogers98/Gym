namespace Gym.Domain._Common
{
    public abstract class DomainEvent
    {
        public DomainEventId Id { get; } 

        public DateTime OccurredOn { get; protected set; }

        protected DomainEvent()
        {
            Id = DomainEventId.Generate();
            OccurredOn = DateTime.UtcNow;
        }

        protected DomainEvent(DomainEventId id, DateTime occurredOn)
        {
            Id = id;
            OccurredOn = occurredOn;
        }
    }
}
