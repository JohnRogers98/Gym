using Gym.Domain.AccountContext.Events;
using Gym.Infrastructure.Scanners;

namespace Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto
{
    [EventSerializationForm<AccountChargedDomainEvent>]
    internal record AccountChargedDto(
        String Id,
        DateTime OccurredOn,
        String UserId,
        Int32 ByCount,
        String? Reason
        );
}
