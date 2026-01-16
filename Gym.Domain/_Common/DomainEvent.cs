namespace Gym.Domain._Common
{
    public abstract class DomainEvent
    {
        public DateTime OccuredOn { get; protected set; }

        protected DomainEvent()
        {
            OccuredOn = DateTime.UtcNow;
        }
    }
}
