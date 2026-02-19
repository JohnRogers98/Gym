using Gym.Domain.AccountContext.Events;
using Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto;

namespace Gym.Infrastructure.EventStores.Serializers
{
    internal partial class EventSerializer
    {
        private AccountCreatedDto ToDto(AccountCreatedDomainEvent domainEvent)
        {
            return new AccountCreatedDto(domainEvent.Id.Value.ToString(), domainEvent.OccuredOn);
        }

        private AccountChargedDto ToDto(AccountChargedDomainEvent domainEvent)
        {
            return new AccountChargedDto(
                domainEvent.Id.Value.ToString(),
                domainEvent.OccuredOn,
                domainEvent.UserId.Value,
                domainEvent.ByCount,
                domainEvent.Reason);
        }

        private TrainingBookedDto ToDto(TrainingBookedDomainEvent domainEvent)
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
