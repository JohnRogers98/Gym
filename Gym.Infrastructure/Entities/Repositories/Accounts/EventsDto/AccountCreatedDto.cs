using Gym.Domain.AccountContext.Events;
using Gym.Infrastructure.Scanners;

namespace Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto
{
    [EventSerializationForm<AccountCreatedDomainEvent>]
    internal record AccountCreatedDto(
        String Id,
        DateTime OccurredOn
        );
}
