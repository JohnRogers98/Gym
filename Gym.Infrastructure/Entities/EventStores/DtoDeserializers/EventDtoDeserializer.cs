using Gym.Infrastructure.Scanners;
using System.Text.Json;

namespace Gym.Infrastructure.Entities.EventStores.DtoDeserializers
{
    internal class EventDtoDeserializer : IEventDtoDeserializer
    {
        private readonly Dictionary<String, Type> _operationToDtoTypeMap = new();


        public EventDtoDeserializer(EventContractScanner eventContractScanner)
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

        public Object Deserialize(EventEntity eventEntity)
        {
            _operationToDtoTypeMap.TryGetValue(eventEntity.Operation, out Type? dtoType);
            if (dtoType is null)
            {
                throw new ArgumentException($"Operation is not declared as deserialized - {eventEntity.Operation}");
            }

            return JsonSerializer.Deserialize(eventEntity.Data, dtoType)
                ?? throw new ArgumentException($"Data is not of declared event type - {eventEntity.Operation}");
        }

        public TDto Deserialize<TDto>(EventEntity eventEntity) where TDto : class
        {
            return this.Deserialize(eventEntity) as TDto
                ?? throw new ArgumentException($"Cannot deserialize event - {eventEntity.Id} in {typeof(TDto)}");
        }
    }
}
