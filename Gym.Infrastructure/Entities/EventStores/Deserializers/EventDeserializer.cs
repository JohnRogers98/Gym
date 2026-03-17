using Gym.Domain._Common;
using Gym.Infrastructure.Scanners;
using System.Text.Json;

namespace Gym.Infrastructure.Entities.EventStores.Deserializers
{
    internal partial class EventDeserializer : IEventDeserializer
    {
        private readonly Dictionary<String, Type> _operationToDtoTypeMap = new();

        public EventDeserializer(EventContractScanner eventContractScanner)
        {
            foreach (var anOperation in eventContractScanner.OperationToDomainMap.Keys)
            {
                this.RegisterMapping(anOperation, eventContractScanner.GetDtoTypeByOperationKey(anOperation)!);
            }
        }

        private void RegisterMapping(String operationKey, Type dtoType)
        {
            _operationToDtoTypeMap.Add(operationKey, dtoType);
        }

        public DomainEvent Deserialize(EventEntity eventEntity)
        {
            _operationToDtoTypeMap.TryGetValue(eventEntity.Operation, out Type? dtoType);
            if (dtoType is null)
            {
                throw new ArgumentException($"Operation is not declared as deserialized - {eventEntity.Operation}");
            }

            Object? dto = JsonSerializer.Deserialize(eventEntity.Data, dtoType)
                ?? throw new ArgumentException($"Data is not of declared event type - {eventEntity.Operation}");

            return ToDomainEvent((dynamic)dto);
        }

        public TDomainEvent Deserialize<TDomainEvent>(EventEntity eventEntity) where TDomainEvent : DomainEvent
        {
            return this.Deserialize(eventEntity) as TDomainEvent 
                ?? throw new ArgumentException($"Cannot deserialize event - {eventEntity.Id} in {typeof(TDomainEvent)}");
        }
    }
}
