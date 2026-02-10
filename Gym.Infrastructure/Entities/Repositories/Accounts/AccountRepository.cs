using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext;
using Gym.Infrastructure.EventStores;
using System.Text.RegularExpressions;

namespace Gym.Infrastructure.Entities.Repositories.Accounts
{
    internal class AccountRepository(IEventStore _eventStore, AccountEventMapper _accountEventMapper) : IAccountRepository
    {

        private readonly Dictionary<Account, Int32> _identityVersionMap = new(ReferenceEqualityComparer.Instance);

        public async Task<Account> GetByIdAsync(AccountId accountId, CancellationToken cancellationToken)
        {
            IEnumerable<EventEntity> entities = await _eventStore.LoadAsync(new StreamId(accountId.Value), cancellationToken);
            
            UserId userId = UserId.From(
                Regex.Replace(accountId.Value, "^account_", ""));
            
            List<DomainEvent> domainEvents = new ();
            foreach (var entity in entities)
            {
                domainEvents.Add(_accountEventMapper.Deserialize(entity));
            }

            Account account = Account.Restore(accountId, userId, domainEvents);

            Int32 lastFetchedVersion = domainEvents.Any() ? entities.Max(x => x.Version) : 0;
            this.UpsertLastKnownVersion(account, lastFetchedVersion);

            return account;
        }

        public async Task SaveAsync(Account account, CancellationToken cancellationToken)
        {
            List<EventEntity> entities = new();

            Int32 lastVersion = this.GetLastKnownVersion(account);
            foreach (var aDomainEvent in account.DomainEvents)
            {
                EventEntity entity = this.CreateEventEntity(account.Id, aDomainEvent, ++lastVersion);
                entities.Add(entity);
            }

            await _eventStore.SaveAsync(new StreamId(account.Id.Value), entities, cancellationToken);

            this.UpsertLastKnownVersion(account, lastVersion);
        }

        private Int32 GetLastKnownVersion(Account account) => _identityVersionMap.GetValueOrDefault(account);

        private void UpsertLastKnownVersion(Account account, Int32 newVersion) => _identityVersionMap[account] = newVersion;

        private EventEntity CreateEventEntity(AccountId accountId, DomainEvent domainEvent, Int32 version)
        {
            return new EventEntity()
            {
                Id = domainEvent.Id.Value.ToString(),
                StreamId = accountId.Value,
                Version = version,
                Operation = domainEvent.GetType().Name,
                Data = _accountEventMapper.Serialize(domainEvent),
                OccurredAt = domainEvent.OccuredOn
            };
        }
    }
}
