using Gym.Abstractions.Query.CalendarEvents;
using Gym.Abstractions.Query.EventStore;
using Gym.Domain.AccountContext.Events;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.DtoDeserializers;
using Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto;

namespace Gym.Infrastructure.Entities.Projections.Events.Account
{
    internal class TrainingCompletedProjectionHandler(
        ICalendarEventProjectionQueryService _calendarEventProjectionQueryService,
        IEventDtoDeserializer _eventDtoDeserializer,
        EventProjectionStore _eventProjectionStore) : IProjectionHandler
    {
        public Boolean CanHandle(String aggregateType, String operation)
        {
            return aggregateType == nameof(Domain.AccountContext.Account) && operation == nameof(TrainingCompletedDomainEvent);
        }

        public async Task HandleAsync(EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var trainingCompletedDto = _eventDtoDeserializer.Deserialize<TrainingCompletedDto>(eventEntity);

            var calendarProjection = await _calendarEventProjectionQueryService.GetByIdAsync(trainingCompletedDto.CalendarEventId, cancellationToken)
                ?? throw new ArgumentNullException();

            var projection = new EventProjection()
            {
                Id = eventEntity.Id,
                StreamId = eventEntity.StreamId,
                Operation = eventEntity.Operation,
                Version = eventEntity.Version,
                OccurredAt = eventEntity.OccurredAt,
                Payload = new()
                {
                    { nameof(TrainingBookedDto.BookingId), trainingCompletedDto.BookingId },
                    { nameof(TrainingBookedDto.CalendarEventId), trainingCompletedDto.CalendarEventId },
                    { "TrainingId", calendarProjection.Training.Id },
                    { "TrainingName", calendarProjection.Training.Name },
                }
            };

            await _eventProjectionStore.CreateAsync(projection, cancellationToken);
        }
    }
}
