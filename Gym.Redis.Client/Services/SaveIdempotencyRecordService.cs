using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace Gym.Redis.Client.Services
{
    public interface ISaveIdempotencyRecordService
    {
        Task<Boolean> HandleAsync(String key, IdempotencyRecord record, When when = When.Always);
    }

    internal class SaveIdempotencyRecordService(IConnectionMultiplexer _connectionMultiplexer, IOptions<RedisOptions> _redisOptions) 
        : ISaveIdempotencyRecordService
    {
        public async Task<Boolean> HandleAsync(String key, IdempotencyRecord record, When when = When.Always)
        {
            var redisDatabase = _connectionMultiplexer.GetDatabase();

            return await redisDatabase.StringSetAsync(
                KeyResolver.ResolveIdempotencyRecordKey(key),
                value: JsonSerializer.Serialize(record),
                expiry: _redisOptions.Value.IdempotencyKeyTtl,
                when: when
            );
        }
    }
}
