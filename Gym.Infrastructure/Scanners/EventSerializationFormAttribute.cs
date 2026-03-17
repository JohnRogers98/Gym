using Gym.Domain._Common;

namespace Gym.Infrastructure.Scanners
{
    internal class EventSerializationFormAttribute<T> : Attribute where T : DomainEvent
    {
        public String? OperationKey { get; set; }
    }
}
