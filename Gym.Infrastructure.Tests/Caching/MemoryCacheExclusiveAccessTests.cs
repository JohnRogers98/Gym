using Gym.Domain._Common;
using Gym.Infrastructure.Caching;

namespace Gym.Infrastructure.Tests.Caching
{
    public class MemoryCacheExclusiveAccessTests
    {
        [Fact]
        public async Task Lock_Exclusive_Operation_And_Remove_Lock_Using_Access_Key()
        {
            MemoryCacheExclusiveAccess sut = new();

            ExclusiveAccessResult lockResult = await sut.TryAcquireAsync("id", "operation", TestContext.Current.CancellationToken);
            Assert.True(lockResult.Result);

            Boolean releaseResult = await sut.ReleaseAsync("id", "operation", lockResult.AccessKey!, TestContext.Current.CancellationToken);
            Assert.True(releaseResult);
        }

        [Fact]
        public async Task Lock_Exclusive_Operation_And_Try_To_Lock_Again()
        {
            MemoryCacheExclusiveAccess sut = new();
            await sut.TryAcquireAsync("id", "operation", TestContext.Current.CancellationToken);

            ExclusiveAccessResult duplicatedLockResult = await sut.TryAcquireAsync("id", "operation", TestContext.Current.CancellationToken);
            Assert.False(duplicatedLockResult.Result);
        }

        [Fact]
        public async Task Lock_Exclusive_Operation_After_Releasing()
        {
            MemoryCacheExclusiveAccess sut = new();
            ExclusiveAccessResult firstLockResult = await sut.TryAcquireAsync("id", "operation", TestContext.Current.CancellationToken);
            await sut.ReleaseAsync("id", "operation", firstLockResult.AccessKey!, TestContext.Current.CancellationToken);

            ExclusiveAccessResult secondLockResult = await sut.TryAcquireAsync("id", "operation", TestContext.Current.CancellationToken);
            Assert.True(secondLockResult.Result);
        }
    }
}
