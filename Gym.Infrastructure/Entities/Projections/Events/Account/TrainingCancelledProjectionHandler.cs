using Gym.Abstractions.Query.CalendarEvents;
using Gym.Abstractions.Query.EventStore;
using Gym.Domain.AccountContext.Events;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.DtoDeserializers;
using Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto;

namespace Gym.Infrastructure.Entities.Projections.Events.Account
{
    internal class TrainingCancelledProjectionHandler(
        ICalendarEventProjectionQueryService _calendarEventProjectionQueryService,
        IEventDtoDeserializer _eventDtoDeserializer,
        EventProjectionStore _eventProjectionStore) : IProjectionHandler
    {
        public Boolean CanHandle(String aggregateType, String operation)
        {
            return aggregateType == nameof(Domain.AccountContext.Account) && operation == nameof(TrainingCancelledDomainEvent);
        }

        public async Task HandleAsync(EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var trainingCancelledDto = _eventDtoDeserializer.Deserialize<TrainingCancelledDto>(eventEntity);

            var calendarProjection = await _calendarEventProjectionQueryService.GetByIdAsync(trainingCancelledDto.CalendarEventId, cancellationToken)
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
                    { nameof(TrainingBookedDto.BookingId), trainingCancelledDto.BookingId },
                    { nameof(TrainingBookedDto.CalendarEventId), trainingCancelledDto.CalendarEventId },
                    { "TrainingId", calendarProjection.Training.Id },
                    { "TrainingName", calendarProjection.Training.Name },
                }
            };

            await _eventProjectionStore.CreateAsync(projection, cancellationToken);
        }
    }
}
