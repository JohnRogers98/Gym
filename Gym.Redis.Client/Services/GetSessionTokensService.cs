using StackExchange.Redis;
using System.Text.Json;

namespace Gym.Redis.Client.Services
{
    public interface IGetSessionTokensService
    {
        Task<SessionTokens?> HandleAsync(String clientSessionKey);
    }

    internal class GetSessionTokensService(IConnectionMultiplexer _connectionMultiplexer) : IGetSessionTokensService
    {
        public async Task<SessionTokens?> HandleAsync(String clientSessionKey)
        {
            var redisDatabase = _connectionMultiplexer.GetDatabase();

            RedisValue value = await redisDatabase.StringGetAsync(KeyResolver.ResolveSessionKey(clientSessionKey));
            if (value.IsNullOrEmpty) 
                return null;

            return JsonSerializer.Deserialize<SessionTokens>(value.ToString());     
        }
    }
}
