using Gym.Domain.CalendarEventAggregate;
using Gym.Infrastructure.Entities.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoConsoleApp.Repositories.CalendarEvents
{
    public class CalendarEventRepository(IMongoCollection<CalendarEventEntity> _calendarEventCollection) : ICalendarEventRepository, ICalendarEventQueryService
    {
        public CalendarEventId NextIdentity() => CalendarEventId.From(ObjectId.GenerateNewId().ToString());

        public async Task SaveAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken)
        {
            CalendarEventEntity calendarEventEntity = calendarEvent.ToEntity();

            await _calendarEventCollection.ReplaceOneAsync(
                eCalendarEvent => eCalendarEvent.Id == calendarEventEntity.Id,
                calendarEventEntity,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
        }

        public async Task<CalendarEvent?> GetByIdAsync(CalendarEventId id, CancellationToken cancellationToken)
        {
            var foundedEntity = await _calendarEventCollection.Find(eCalendarEvent => eCalendarEvent.Id == id.Value.ToObjectId())
                .FirstOrDefaultAsync(cancellationToken);

            return foundedEntity is not null ? foundedEntity.ToDomain() : null; 
        }

        public async Task<IEnumerable<CalendarEvent>> GetAllAsync(CancellationToken cancellationToken)
        {
            List<CalendarEvent> allCalendarEvents = new();

            await _calendarEventCollection.Find(Builders<CalendarEventEntity>.Filter.Empty)
                .ForEachAsync(eCalendarEvent => allCalendarEvents.Add(eCalendarEvent.ToDomain()));

            return allCalendarEvents;
        }

        public async Task<Boolean> ExistsAsync(CalendarEventId id, CancellationToken cancellationToken) 
            => await _calendarEventCollection.Find(eCalendarEvent => eCalendarEvent.Id == id.Value.ToObjectId()).AnyAsync(cancellationToken);

    }
}
