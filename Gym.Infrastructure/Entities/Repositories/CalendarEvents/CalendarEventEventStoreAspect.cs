using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.Serializers;

namespace Gym.Infrastructure.Entities.Repositories.CalendarEvents
{
    internal class CalendarEventEventStoreAspect(
        ICalendarEventRepository _decoratee,
        IEventStore _eventStore,
        IEventSerializer _eventSerializer) : ICalendarEventRepository
    {
        public async Task<Boolean> ExistsAsync(CalendarEventId id, CancellationToken cancellationToken)
        {
            return await _decoratee.ExistsAsync(id, cancellationToken);
        }

        public async Task<CalendarEvent?> GetByIdAsync(CalendarEventId id, CancellationToken cancellationToken)
        {
            return await _decoratee.GetByIdAsync(id, cancellationToken);
        }

        public CalendarEventId NextIdentity()
        {
            return _decoratee.NextIdentity();
        }

        public async Task SaveAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken)
        {
            if (calendarEvent.DomainEvents.Any())
            {
                await _eventStore.SaveAutoversionedAsync(
                    this.CreateStreamId(calendarEvent.Id),
                    calendarEvent.DomainEvents.Select(domainEvent => this.CreateEventEntity(calendarEvent.Id, domainEvent)).ToList(),
                    cancellationToken
                    );
            }
            await _decoratee.SaveAsync(calendarEvent, cancellationToken);

            calendarEvent.ClearDomainEvents();
        }

        private EventEntity CreateEventEntity(CalendarEventId calendarEventId, DomainEvent domainEvent)
        {
            return new EventEntity()
            {
                Id = domainEvent.Id.Value.ToString(),
                StreamId = this.CreateStreamId(calendarEventId).Value,
                AggregateType = nameof(CalendarEvent),
                Operation = domainEvent.GetType().Name,
                Data = _eventSerializer.Serialize(domainEvent),
                OccurredAt = domainEvent.OccurredOn
            };
        }

        private StreamId CreateStreamId(CalendarEventId calendarEventId)
            => new StreamId($"calendarEvent_{calendarEventId.Value}");
    }
}
