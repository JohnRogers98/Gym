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
                dto.occurredOn,
                UserId.From(dto.UserId),
                dto.ByCount,
                dto.Reason
                );
        }

        private DomainEvent ToDomainEvent(AccountCreatedDto dto)
        {
            return AccountCreatedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.occurredOn
                );
        }

        private DomainEvent ToDomainEvent(TrainingBookedDto dto)
        {
            return TrainingBookedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.occurredOn,
                BookingId.From(dto.BookingId),
                UserId.From(dto.UserId),
                CalendarEventId.From(dto.CalendarEventId)
                );
        }
    }
}
