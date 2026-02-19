using Gym.Abstractions.Query.EventStore;
using Gym.Domain.AccountContext;

namespace Gym.Application.Services.AccountApi
{
    internal static class AccountExtensions
    {
        public static AccountDetails ToDetails(this Account account) => new AccountDetails(account.AvailableTrainingsCount);

        public static IEnumerable<AccountEventDetails> ToDetails(this IEnumerable<EventProjection> eventProjections)
        {
            return eventProjections.Select(aProjection => new AccountEventDetails(
                aProjection.Version,
                aProjection.Operation,
                aProjection.OccurredAt, 
                aProjection.Payload)
            );
        }
    }
}
