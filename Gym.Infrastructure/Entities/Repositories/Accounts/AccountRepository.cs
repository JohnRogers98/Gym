using Gym.Application.Extensions;
using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext;
using Gym.Domain.AccountContext.ValueObjects;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.Deserializers;
using Gym.Infrastructure.Entities.EventStores.Serializers;
using System.Text.RegularExpressions;

namespace Gym.Infrastructure.Entities.Repositories.Accounts
{
    internal class AccountRepository(IEventStore _eventStore, IEventSerializer _eventSerializer, IEventDeserializer _eventDeserializer) : IAccountRepository
    {

        private readonly Dictionary<Account, Int32> _identityVersionMap = new(ReferenceEqualityComparer.Instance);

        public async Task<Account> GetByIdAsync(AccountId accountId, CancellationToken cancellationToken)
        {
            IEnumerable<EventEntity> entities = await _eventStore.LoadAsync(new StreamId(accountId.Value), cancellationToken);
            
            List<DomainEvent> domainEvents = new ();
            foreach (var entity in entities)
            {
                domainEvents.Add(_eventDeserializer.Deserialize(entity));
            }

            UserId userId = UserId.From(Regex.Replace(accountId.Value, "^account_", "")).Unwrap();
            Account account = Account.Restore(accountId, userId, domainEvents);

            Int32 lastFetchedVersion = domainEvents.Any() ? entities.Max(x => x.Version) : 0;
            this.UpsertLastFetchedVersion(account, lastFetchedVersion);

            return account;
        }

        public async Task SaveAsync(Account account, CancellationToken cancellationToken)
        {
            List<EventEntity> entities = new();

            Int32 newVersion = this.GetLastFetchedVersion(account);
            foreach (var aDomainEvent in account.DomainEvents)
            {
                newVersion++;
                EventEntity entity = this.CreateEventEntity(account.Id, aDomainEvent, newVersion);
                entities.Add(entity);
            }

            await _eventStore.SaveVersionedAsync(new StreamId(account.Id.Value), entities, this.GetLastFetchedVersion(account), cancellationToken);

            this.UpsertLastFetchedVersion(account, newVersion);

            account.ClearDomainEvents();
        }

        private Int32 GetLastFetchedVersion(Account account) => _identityVersionMap.GetValueOrDefault(account);

        private void UpsertLastFetchedVersion(Account account, Int32 newVersion) => _identityVersionMap[account] = newVersion;

        private EventEntity CreateEventEntity(AccountId accountId, DomainEvent domainEvent, Int32 version)
        {
            return new EventEntity()
            {
                Id = domainEvent.Id.Value.ToString(),
                StreamId = accountId.Value,
                Version = version,
                AggregateType = nameof(Account),
                Operation = domainEvent.GetType().Name,
                Data = _eventSerializer.Serialize(domainEvent),
                OccurredAt = domainEvent.OccurredOn
            };
        }
    }
}
