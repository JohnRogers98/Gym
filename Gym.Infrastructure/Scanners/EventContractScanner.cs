using Gym.Domain._Common;
using System.Reflection;

namespace Gym.Infrastructure.Scanners
{
    internal class EventContractScanner
    {
        private Dictionary<String, Type> _operationToDomainMap = new();
        private Dictionary<Type, Type> _domainToDtoMap = new();

        public IReadOnlyDictionary<String, Type> OperationToDomainMap => _operationToDomainMap;
        public IReadOnlyDictionary<Type, Type> DomainToDtoMap => _domainToDtoMap;

        public Type? GetDomainTypeByOperationKey(String operationKey)
               => _operationToDomainMap.TryGetValue(operationKey, out var domain) ? domain : null;

        public Type? GetDtoTypeByDomainType(Type domainType)
            => _domainToDtoMap.TryGetValue(domainType, out var dto) ? dto : null;

        public Type? GetDtoTypeByOperationKey(String operationKey)
        {
            var domain = this.GetDomainTypeByOperationKey(operationKey);
            return domain != null ? this.GetDtoTypeByDomainType(domain) : null;
        }

        private EventContractScanner() { }

        public static EventContractScanner ScanAssembly(Assembly assembly, Type? serializer = null, Type? deserializer = null)
        {
            EventContractScanner eventSerializationContainer = new();

            assembly.GetTypes().ToList().ForEach(eventSerializationContainer.AddMapping);

            if (serializer is not null)
                eventSerializationContainer.CheckExistenceOfSerializerMethods(serializer);

            if (deserializer is not null)
                eventSerializationContainer.CheckExistenceOfDeserializerMethods(deserializer);

            return eventSerializationContainer;
        }

        private void AddMapping(Type dtoType)
        {
            foreach (Attribute anAttribute in dtoType.GetCustomAttributes())
            {
                if (anAttribute.GetType().IsGenericType && anAttribute.GetType().GetGenericTypeDefinition() == typeof(EventSerializationFormAttribute<>))
                {
                    Type domainEventType = anAttribute.GetType().GetGenericArguments()[0];

                    var operationKey = anAttribute.GetType()
                        .GetProperty(nameof(EventSerializationFormAttribute<>.OperationKey))?
                        .GetValue(anAttribute) as String ?? domainEventType.Name;

                    _operationToDomainMap.Add(operationKey, domainEventType);
                    _domainToDtoMap.Add(domainEventType, dtoType);
                }
            }
        }

        private void CheckExistenceOfSerializerMethods(Type serializer)
        {
            foreach (var pair in _domainToDtoMap)
            {
                var method = serializer.GetMethod("ToDto",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                    new[] { pair.Key });

                if (method is null || method.ReturnType != pair.Value)
                    throw new NotImplementedException($"Serialzer does not contain ToDto method for domain type - {pair.Key} or return value is not of type {pair.Value}.");
            }
        }

        private void CheckExistenceOfDeserializerMethods(Type deserializer)
        {
            foreach (var pair in _domainToDtoMap)
            {
                var method = deserializer.GetMethod("ToDomainEvent",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                    new[] { pair.Value });

                if (method is null || (method.ReturnType != pair.Key && method.ReturnType != typeof(DomainEvent)))
                    throw new NotImplementedException($"Deserialzer does not contain ToDomainEvent method for type - {pair.Value} or return value is not of type {pair.Key}.");
            }
        }

    }
}
