using Gym.Domain._Common;
using Gym.Domain.AccountContext.Events;
using Gym.Infrastructure.EventStores;

namespace Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto
{
    internal record TrainingBookedDto(String Id, DateTime OccuredOn, String BookingId, String UserId, String CalendarEventId) : EventDto
    {
        public override DomainEvent ToDomainEvent()
        {
            return TrainingBookedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(Id)),
                OccuredOn,
                Domain.AccountContext.BookingId.From(BookingId),
                Domain._Shared.UserId.From(UserId),
                Domain._Shared.CalendarEventId.From(CalendarEventId)
                );
        }

        public static TrainingBookedDto FromDomainEvent(TrainingBookedDomainEvent domainEvent)
        {
            return new TrainingBookedDto(
                domainEvent.Id.Value.ToString(),
                domainEvent.OccuredOn,
                domainEvent.BookingId.Value,
                domainEvent.UserId.Value,
                domainEvent.CalendarEventId.Value);
        }
    }
}
