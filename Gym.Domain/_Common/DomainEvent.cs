namespace Gym.Domain._Common
{
    public abstract class DomainEvent
    {
        public DomainEventId Id { get; } 

        public DateTime OccuredOn { get; protected set; }

        protected DomainEvent()
        {
            Id = DomainEventId.Generate();
            OccuredOn = DateTime.UtcNow;
        }

        protected DomainEvent(DomainEventId id, DateTime occuredOn)
        {
            Id = id;
            OccuredOn = occuredOn;
        }
    }
}
