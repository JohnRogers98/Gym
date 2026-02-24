using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext;
using Gym.Domain.AccountContext.Events;
using Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto;

namespace Gym.Infrastructure.Entities.EventStores.Deserializers
{
    internal partial class EventDeserializer
    {
        private DomainEvent ToDomainEvent(AccountChargedDto dto)
        {
            return AccountChargedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.OccurredOn,
                UserId.From(dto.UserId),
                dto.ByCount,
                dto.Reason
                );
        }

        private DomainEvent ToDomainEvent(AccountCreatedDto dto)
        {
            return AccountCreatedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.OccurredOn
                );
        }

        private DomainEvent ToDomainEvent(TrainingBookedDto dto)
        {
            return TrainingBookedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.OccurredOn,
                BookingId.From(dto.BookingId),
                UserId.From(dto.UserId),
                CalendarEventId.From(dto.CalendarEventId)
                );
        }

        private DomainEvent ToDomainEvent(TrainingCompletedDto dto)
        {
            return TrainingCompletedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.OccurredOn,
                BookingId.From(dto.BookingId),
                UserId.From(dto.UserId),
                CalendarEventId.From(dto.CalendarEventId)
                );
        }
    }
}
