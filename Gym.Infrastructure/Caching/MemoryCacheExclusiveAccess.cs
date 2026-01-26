using Gym.Domain._Common;
using System.Collections.Concurrent;

namespace Gym.Infrastructure.Caching
{
    internal class MemoryCacheExclusiveAccess : IExclusiveAccessCoordinator
    {
        private static readonly ConcurrentDictionary<ExclusiveAccessKey, Boolean> _exclusiveAccessLock = new();

        public async Task<ExclusiveAccessResult> TryAcquireAsync(String aggregateId, String operation, CancellationToken cancellationToken = default)
        {
            String lockedOperationKey = $"{aggregateId}:{operation}";
            Guid accessKey = Guid.NewGuid();
            ExclusiveAccessKey exclusiveAccessKey = new(lockedOperationKey, accessKey);

            _exclusiveAccessLock.TryAdd(exclusiveAccessKey, true);

            return ExclusiveAccessResult.Successful(accessKey);
        }

        public async Task<Boolean> ReleaseAsync(String aggregateId, String operation, Guid accessKey, CancellationToken cancellationToken = default)
        {
            String lockedOperationKey = $"{aggregateId}:{operation}";
            ExclusiveAccessKey exclusiveAccessKey = new(lockedOperationKey, accessKey);
            return _exclusiveAccessLock.Remove(exclusiveAccessKey, out _);
        }

        private record ExclusiveAccessKey(String lockedOperationKey, Guid accessKey);
    }
}
