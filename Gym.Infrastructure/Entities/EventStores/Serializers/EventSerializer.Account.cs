using Gym.Domain.AccountContext.Events;
using Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto;

namespace Gym.Infrastructure.Entities.EventStores.Serializers
{
    internal partial class EventSerializer
    {
        private AccountCreatedDto ToDto(AccountCreatedDomainEvent domainEvent)
        {
            return new AccountCreatedDto(domainEvent.Id.Value.ToString(), domainEvent.OccurredOn);
        }

        private AccountChargedDto ToDto(AccountChargedDomainEvent domainEvent)
        {
            return new AccountChargedDto(
                domainEvent.Id.Value.ToString(),
                domainEvent.OccurredOn,
                domainEvent.UserId.Value,
                domainEvent.ByCount,
                domainEvent.Reason);
        }

        private TrainingBookedDto ToDto(TrainingBookedDomainEvent domainEvent)
        {
            return new TrainingBookedDto(
                domainEvent.Id.Value.ToString(),
                domainEvent.OccurredOn,
                domainEvent.BookingId.Value,
                domainEvent.UserId.Value,
                domainEvent.CalendarEventId.Value);
        }
    }
}
