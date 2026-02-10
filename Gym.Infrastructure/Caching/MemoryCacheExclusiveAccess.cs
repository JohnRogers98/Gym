using Gym.Domain._Common;
using System.Collections.Concurrent;

namespace Gym.Infrastructure.Caching
{
    internal class MemoryCacheExclusiveAccess : IExclusiveAccessCoordinator
    {
        private readonly ConcurrentDictionary<ResourceOperation, AccessKey> _exclusiveAccessLock = new();

        public async Task<ExclusiveAccessResult> TryAcquireAsync(String aggregateId, String operation, CancellationToken cancellationToken = default)
        {
            ResourceOperation resourceOperation = new($"{aggregateId}:{operation}");
            AccessKey accessKey = new(Guid.NewGuid().ToString());

            var isSuccess = _exclusiveAccessLock.TryAdd(resourceOperation, accessKey);

            if (isSuccess)
                return ExclusiveAccessResult.Successful(accessKey.Value);
            else
                return ExclusiveAccessResult.Denied();
        }

        public async Task<Boolean> ReleaseAsync(String aggregateId, String operation, String accessKey, CancellationToken cancellationToken = default)
        {
            ResourceOperation resourceOperation = new($"{aggregateId}:{operation}");

            KeyValuePair<ResourceOperation, AccessKey> lockPair = new(
                new ResourceOperation($"{aggregateId}:{operation}"), new AccessKey(accessKey)); 

            return _exclusiveAccessLock.TryRemove(lockPair);
        }

        private record ResourceOperation(String Value);

        private record AccessKey(String Value);
    }
}
