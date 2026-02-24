using Gym.Domain._Common;
using Gym.Domain.AccountContext.Events;
using Gym.Domain.CalendarEventContext.Events;
using Gym.Domain.ClientContext.Events;
using Gym.Domain.InstructorContext.Events;
using Gym.Domain.TrainingContext.Events;
using Gym.Domain.UserContext.Events;
using Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto;
using Gym.Infrastructure.Entities.Repositories.CalendarEvents.EventsDto;
using Gym.Infrastructure.Entities.Repositories.Clients.EventsDto;
using Gym.Infrastructure.Entities.Repositories.Instructors.EventsDto;
using Gym.Infrastructure.Entities.Repositories.Trainings.EventsDto;
using Gym.Infrastructure.Entities.Repositories.Users.EventsDto;
using System.Text.Json;

namespace Gym.Infrastructure.Entities.EventStores.Deserializers
{
    internal partial class EventDeserializer : IEventDeserializer
    {
        private readonly Dictionary<String, Type> _operationToDtoType = new()
        {
            [nameof(AccountChargedDomainEvent)] = typeof(AccountChargedDto),
            [nameof(AccountCreatedDomainEvent)] = typeof(AccountCreatedDto),
            [nameof(TrainingBookedDomainEvent)] = typeof(TrainingBookedDto),
            [nameof(TrainingCompletedDomainEvent)] = typeof(TrainingCompletedDto),
            [nameof(ClientCreatedDomainEvent)] = typeof(ClientCreatedDto),
            [nameof(UserCreatedDomainEvent)] = typeof(UserCreatedDto),
            [nameof(CalendarEventCreatedDomainEvent)] = typeof(CalendarEventCreatedDto),
            [nameof(CalendarEventBookedDomainEvent)] = typeof(CalendarEventBookedDto),
            [nameof(InstructorCreatedDomainEvent)] = typeof(InstructorCreatedDto),
            [nameof(TrainingCreatedDomainEvent)] = typeof(TrainingCreatedDto),
            [nameof(CalendarEventCompletedDomainEvent)] = typeof(CalendarEventCompletedDto),
            [nameof(CalendarEventCancelledDomainEvent)] = typeof(CalendarEventCancelledDto)
        };

        public DomainEvent Deserialize(EventEntity eventEntity)
        {
            _operationToDtoType.TryGetValue(eventEntity.Operation, out Type? dtoType);
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
