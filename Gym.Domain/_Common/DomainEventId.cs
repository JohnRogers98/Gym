namespace Gym.Domain._Common
{
    public record DomainEventId
    {
        public Guid Value { get; }

        private DomainEventId(Guid value) => Value = value;

        public static DomainEventId From(Guid id) => new(id);

        public static DomainEventId Generate() => new(Guid.NewGuid());
    }
}
