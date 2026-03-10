using Gym.Abstractions.Query.CalendarEvents;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.CalendarEventContext.Events;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.DtoDeserializers;
using Gym.Infrastructure.Entities.Repositories.CalendarEvents.EventsDto;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.CalendarEvents
{
    internal class CalendarEventCancelledProjectionHandler(
        IMongoCollection<CalendarEventProjection> _projectionCollection,
        MongoUnitOfWork _mongoUnitOfWork,
        IEventDtoDeserializer _eventDtoDeserializer) : IProjectionHandler
    {
        public Boolean CanHandle(String aggregateType, String operation)
        {
            return aggregateType == nameof(CalendarEvent) && operation == nameof(CalendarEventCancelledDomainEvent);
        }

        public async Task HandleAsync(EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var calendarEventCancelledDto = _eventDtoDeserializer.Deserialize<CalendarEventCancelledDto>(eventEntity);

            await _projectionCollection.UpdateOneAsync(
               _mongoUnitOfWork.Session,
               projection => projection.Id == calendarEventCancelledDto.CalendarEventId,
               Builders<CalendarEventProjection>.Update.Set(x => x.Status, CalendarEventStatus.Cancelled.ToString()),
               cancellationToken: cancellationToken);
        }
    }
}
