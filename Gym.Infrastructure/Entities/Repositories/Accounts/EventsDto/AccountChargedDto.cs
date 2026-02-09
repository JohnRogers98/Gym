using Gym.Domain._Common;
using Gym.Domain.AccountContext.Events;
using Gym.Infrastructure.EventStores;

namespace Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto
{
    internal record AccountChargedDto(String Id, DateTime OccuredOn, String UserId, Int32 ByCount, String? Reason) : EventDto
    {

        public override DomainEvent ToDomainEvent()
        {
            return AccountChargedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(Id)),
                OccuredOn, 
                Domain._Shared.UserId.From(UserId),
                ByCount,
                Reason);
        }

        public static AccountChargedDto FromDomainEvent(AccountChargedDomainEvent domainEvent)
        {
            return new AccountChargedDto(
                domainEvent.Id.Value.ToString(),
                domainEvent.OccuredOn,
                domainEvent.UserId.Value,
                domainEvent.ByCount,
                domainEvent.Reason);
        }
    }
}
