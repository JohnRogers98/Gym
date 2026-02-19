using Gym.Domain._Common;
using Gym.Domain.AccountContext.Events;
using Gym.Domain.ClientContext.Events;
using Gym.Domain.UserContext.Events;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.Deserializers;
using Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto;
using Gym.Infrastructure.Entities.Repositories.Clients.EventsDto;
using Gym.Infrastructure.Entities.Repositories.Users.EventsDto;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json;

namespace Gym.Infrastructure.EventStores.Deserializers
{
    internal partial class EventDeserializer : IEventDeserializer
    {
        private readonly Dictionary<String, Type> _operationToDtoType = new()
        {
            [nameof(AccountChargedDomainEvent)] = typeof(AccountChargedDto),
            [nameof(AccountCreatedDomainEvent)] = typeof(AccountCreatedDto),
            [nameof(TrainingBookedDomainEvent)] = typeof(TrainingBookedDto),
            [nameof(ClientCreatedDomainEvent)] = typeof(ClientCreatedDto),
            [nameof(UserCreatedDomainEvent)] = typeof(UserCreatedDto)
        };

        public DomainEvent Deserialize(EventEntity eventEntity)
        {
            _operationToDtoType.TryGetValue(eventEntity.Operation, out Type? dtoType);
            if (dtoType is null)
            {
                throw new ArgumentException($"Operation is not declared as deserialized - {eventEntity.Operation}");
            }

            Object? dto = JsonSerializer.Deserialize(eventEntity.Data, dtoType);
            if (dto is null)
            {
                throw new ArgumentException($"Data is not of declared event type - {eventEntity.Operation}");
            }

            return ToDomainEvent((dynamic)dto);
        }

        public TDomainEvent Deserialize<TDomainEvent>(EventEntity eventEntity) where TDomainEvent : DomainEvent
        {
            return this.Deserialize(eventEntity) as TDomainEvent 
                ?? throw new ArgumentException($"Cannot deserialize event - {eventEntity.Id} in {typeof(TDomainEvent)}");
        }
    }
}
