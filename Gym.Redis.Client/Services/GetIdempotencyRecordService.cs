using StackExchange.Redis;
using System.Text.Json;

namespace Gym.Redis.Client.Services
{
    public interface IGetIdempotencyRecordService
    {
        Task<IdempotencyRecord?> HandleAsync(String key);
    }

    public class GetIdempotencyRecordService(IConnectionMultiplexer _connectionMultiplexer) : IGetIdempotencyRecordService
    {
        public async Task<IdempotencyRecord?> HandleAsync(String key)
        {
            var redisDatabase = _connectionMultiplexer.GetDatabase();

            RedisValue value = await redisDatabase.StringGetAsync(KeyResolver.ResolveIdempotencyRecordKey(key));
            if (value.IsNullOrEmpty) return null;

            return JsonSerializer.Deserialize<IdempotencyRecord>(value.ToString());
        }
    }
}
