using Gym.Domain._Common;
using Gym.Domain.AccountContext.Events;
using Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto;
using Gym.Infrastructure.EventStores;
using System.Text.Json;

namespace Gym.Infrastructure.Entities.Repositories.Accounts
{
    internal class AccountEventMapper : EventMapper
    {
        private readonly Dictionary<String, Type> _eventTypeMap = new()
        {
            [nameof(AccountChargedDomainEvent)] = typeof(AccountChargedDto),
            [nameof(AccountCreatedDomainEvent)] = typeof(AccountCreatedDto),
            [nameof(TrainingBookedDomainEvent)] = typeof(TrainingBookedDto)
        };

        public override DomainEvent Deserialize(EventEntity eventEntity)
        {
            _eventTypeMap.TryGetValue(eventEntity.Operation, out Type? eventType);
            if (eventType is null)
            {
                throw new ArgumentException($"Operation is not declared - {eventEntity.Operation}");
            }

            EventDto? eventData = JsonSerializer.Deserialize(eventEntity.Data, eventType) as EventDto;
            if (eventData is null)
            {
                throw new ArgumentException($"Operation is not of declared event type - {eventEntity.Operation}");
            }

            return eventData.ToDomainEvent();
        }

        public override String Serialize(DomainEvent domainEvent)
        {
            EventDto? eventData = null;

            if (domainEvent is AccountChargedDomainEvent accountCharged)
                eventData = AccountChargedDto.FromDomainEvent(accountCharged);

            if (domainEvent is TrainingBookedDomainEvent trainingBooked)
                eventData = TrainingBookedDto.FromDomainEvent(trainingBooked);

            if (domainEvent is AccountCreatedDomainEvent accountCreated)
                eventData = AccountCreatedDto.FromDomainEvent(accountCreated);

            if(eventData is null)
            {
                throw new NotImplementedException();
            }

            return JsonSerializer.Serialize(eventData, eventData.GetType());
        }
    }
}
