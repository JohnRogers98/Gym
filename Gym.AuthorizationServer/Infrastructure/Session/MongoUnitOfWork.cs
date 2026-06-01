using MongoDB.Driver;

namespace Gym.AuthorizationServer.Infrastructure.Session
{
    public class MongoUnitOfWork : IUnitOfWork
    {
        private IClientSessionHandle _session;
        private Boolean _disposed;

        internal IClientSessionHandle? Session => _session;

        private ClientSessionOptions ClientSessionOptions => new();

        public MongoUnitOfWork(IMongoClient _client)
        {
            _session = _client.StartSession(ClientSessionOptions);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_session.IsInTransaction)
            {
                throw new InvalidOperationException("Transaction already started");
            }

            _session.StartTransaction();
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_session?.IsInTransaction == true)
            {
                await _session.CommitTransactionAsync();
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_session?.IsInTransaction == true)
            {
                await _session.AbortTransactionAsync();
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_session?.IsInTransaction == true)
                {
                    _session.AbortTransaction();
                }
                _session?.Dispose();
                _disposed = true;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                if (_session?.IsInTransaction == true)
                {
                    await _session.AbortTransactionAsync();
                }
                _session?.Dispose();
                _disposed = true;
            }
        }
    }
}
