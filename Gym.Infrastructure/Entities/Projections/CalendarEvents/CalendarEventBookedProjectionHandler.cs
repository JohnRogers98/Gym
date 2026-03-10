using Gym.Abstractions.Query.CalendarEvents;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.CalendarEventContext.Events;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.DtoDeserializers;
using Gym.Infrastructure.Entities.Repositories.CalendarEvents.EventsDto;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.CalendarEvents
{
    internal class CalendarEventBookedProjectionHandler(
        IMongoCollection<CalendarEventProjection> _projectionCollection,
        MongoUnitOfWork _mongoUnitOfWork,
        IEventDtoDeserializer _eventDtoDeserializer) : IProjectionHandler
    {
        public Boolean CanHandle(String aggregateType, String operation)
        {
            return aggregateType == nameof(CalendarEvent) && operation == nameof(CalendarEventBookedDomainEvent);
        }

        public async Task HandleAsync(EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var calendarEventBookedDto = _eventDtoDeserializer.Deserialize<CalendarEventBookedDto>(eventEntity);

            await _projectionCollection.UpdateOneAsync(
               _mongoUnitOfWork.Session,
               projection => projection.Id == calendarEventBookedDto.CalendarEventId,
               Builders<CalendarEventProjection>.Update .Push(projection => projection.BookingUsers, new BookingUserInfo(calendarEventBookedDto.UserId)),
               cancellationToken: cancellationToken);
        }
    }
}
