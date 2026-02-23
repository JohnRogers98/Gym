using MongoDB.Bson;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Outbox
{
    internal interface IOutboxResumeTokenStore
    {
        Task<ResumeToken?> GetAsync(CancellationToken cancellationToken = default);
        Task SaveAsync(ResumeToken token, CancellationToken cancellationToken = default);
    }

    internal class OutboxResumeTokenStore(IMongoCollection<OutboxChangeStreamState> _stateCollection) : IOutboxResumeTokenStore
    {
        private ResumeToken? _cachedResumeToken;
        private readonly String _stateId = "outbox_resume_token";

        public async Task<ResumeToken?> GetAsync(CancellationToken cancellationToken = default)
        {
            if (_cachedResumeToken is not null)
                return _cachedResumeToken;

            var resumeTokenState = await _stateCollection.Find(x => x.Id == _stateId)
                .FirstOrDefaultAsync(cancellationToken);

            if(resumeTokenState?.ResumeToken is not null)
                _cachedResumeToken = ResumeToken.From(resumeTokenState.ResumeToken);
            
            return _cachedResumeToken;
        }

        public async Task SaveAsync(ResumeToken token, CancellationToken cancellationToken = default)
        {
            await _stateCollection.UpdateOneAsync(
                entity => entity.Id == _stateId,
                Builders<OutboxChangeStreamState>.Update.Set(x => x.ResumeToken, token.Value).Set(x => x.UpdatedAt, DateTime.UtcNow),
                options: new UpdateOptions { IsUpsert = true },
                cancellationToken: cancellationToken);

            _cachedResumeToken = token;
        }
    }

    internal class OutboxChangeStreamState
    {
        public required String Id { get; set; }
        public required BsonDocument ResumeToken { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    internal record ResumeToken(BsonDocument Value)
    {
        public static ResumeToken From(BsonDocument value) => new(value);
    }
}
