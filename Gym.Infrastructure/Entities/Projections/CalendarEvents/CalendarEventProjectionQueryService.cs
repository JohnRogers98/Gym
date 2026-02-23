using Gym.Abstractions.Query.CalendarEvents;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.CalendarEvents
{
    internal class CalendarEventProjectionQueryService(IMongoCollection<CalendarEventProjection> _projectionCollection) : ICalendarEventProjectionQueryService
    {
        public async Task<IEnumerable<CalendarEventProjection>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _projectionCollection.Find(Builders<CalendarEventProjection>.Filter.Empty)
                .ToListAsync(cancellationToken);
        }

        public async Task<CalendarEventProjection?> GetByIdAsync(String calendarEventId, CancellationToken cancellationToken)
        {
            return await _projectionCollection.Find(projection => projection.Id == calendarEventId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<CalendarEventProjection>> GetStartingFromAsync(DateTime dateTime, CancellationToken cancellationToken)
        {
            return await _projectionCollection.Find(projection => projection.Start >= dateTime)
                .ToListAsync(cancellationToken);
        }
    }
}
