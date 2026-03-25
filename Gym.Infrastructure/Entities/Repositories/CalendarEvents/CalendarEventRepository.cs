using Gym.Application.Extensions;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.CalendarEventContext.ValueObjects;
using Gym.Infrastructure.Entities.Extensions;
using Gym.Infrastructure.Entities.Extensions.Mappings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Repositories.CalendarEvents
{
    internal class CalendarEventRepository(IMongoCollection<CalendarEventEntity> _calendarEventCollection, MongoUnitOfWork _mongoUnitOfWork) : ICalendarEventRepository, IPastCalendarEventsFinder
    {
        public CalendarEventId NextIdentity() => CalendarEventId.From(ObjectId.GenerateNewId().ToString()).Unwrap();

        public async Task SaveAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken)
        {
            CalendarEventEntity calendarEventEntity = calendarEvent.ToEntity();

            await _calendarEventCollection.ReplaceOneAsync(
                _mongoUnitOfWork.Session,
                eCalendarEvent => eCalendarEvent.Id == calendarEventEntity.Id,
                calendarEventEntity,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
        }

        public async Task<CalendarEvent?> GetByIdAsync(CalendarEventId id, CancellationToken cancellationToken)
        {
            var foundedEntity = await _calendarEventCollection.Find(_mongoUnitOfWork.Session, eCalendarEvent => eCalendarEvent.Id == id.Value.ToObjectId())
                .FirstOrDefaultAsync(cancellationToken);

            return foundedEntity?.ToDomain();
        }

        public async Task<Boolean> ExistsAsync(CalendarEventId id, CancellationToken cancellationToken) 
            => await _calendarEventCollection.Find(_mongoUnitOfWork.Session, eCalendarEvent => eCalendarEvent.Id == id.Value.ToObjectId()).AnyAsync(cancellationToken);

        public async Task<IEnumerable<CalendarEvent>> GetPastCalendarEventsAsync(DateTime currentDateTime, CancellationToken cancellationToken)
        {
            var foundedEntities = await _calendarEventCollection.Find(
                    _mongoUnitOfWork.Session,
                    eCalendarEvent => eCalendarEvent.Start < currentDateTime && eCalendarEvent.Status == CalendarEventStatus.Upcoming.ToString())
              .ToListAsync(cancellationToken);

            return foundedEntities.Select(aFoundedEntity => aFoundedEntity.ToDomain()).ToList();
        }
    }
}
