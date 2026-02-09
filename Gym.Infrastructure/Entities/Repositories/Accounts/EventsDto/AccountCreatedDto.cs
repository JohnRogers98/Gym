using Gym.Domain._Common;
using Gym.Domain.AccountContext.Events;
using Gym.Infrastructure.EventStores;

namespace Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto
{
    internal record AccountCreatedDto(String Id, DateTime OccuredOn) : EventDto
    {
        public override DomainEvent ToDomainEvent()
        {
            return AccountCreatedDomainEvent.Restore(DomainEventId.From(Guid.Parse(Id)),OccuredOn);
        }

        public static AccountCreatedDto FromDomainEvent(AccountCreatedDomainEvent domainEvent)
        {
            return new AccountCreatedDto(domainEvent.Id.Value.ToString(), domainEvent.OccuredOn);
        }
    }
}
