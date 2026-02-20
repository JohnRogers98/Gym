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

namespace Gym.Infrastructure.Entities.EventStores.DtoDeserializers
{
    internal class EventDtoDeserializer : IEventDtoDeserializer
    {
        private readonly Dictionary<String, Type> _operationToDtoType = new()
        {
            [nameof(AccountChargedDomainEvent)] = typeof(AccountChargedDto),
            [nameof(AccountCreatedDomainEvent)] = typeof(AccountCreatedDto),
            [nameof(TrainingBookedDomainEvent)] = typeof(TrainingBookedDto),
            [nameof(ClientCreatedDomainEvent)] = typeof(ClientCreatedDto),
            [nameof(UserCreatedDomainEvent)] = typeof(UserCreatedDto),
            [nameof(CalendarEventCreatedDomainEvent)] = typeof(CalendarEventCreatedDto),
            [nameof(CalendarEventBookedDomainEvent)] = typeof(CalendarEventBookedDto),
            [nameof(InstructorCreatedDomainEvent)] = typeof(InstructorCreatedDto),
            [nameof(TrainingCreatedDomainEvent)] = typeof(TrainingCreatedDto)
        };

        public Object Deserialize(EventEntity eventEntity)
        {
            _operationToDtoType.TryGetValue(eventEntity.Operation, out Type? dtoType);
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
