using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace Gym.Redis.Client.Services
{
    public interface ISaveSessionTokensService
    {
        Task HandleAsync(String clientSessionKey, SessionTokens sessionTokens);
    }

    internal class SaveSessionTokensService(IConnectionMultiplexer _connectionMultiplexer, IOptions<RedisOptions> _redisOptions) : ISaveSessionTokensService
    {
        public async Task HandleAsync(String clientSessionKey, SessionTokens sessionTokens)
        {
            var redisDatabase = _connectionMultiplexer.GetDatabase();

            String jsonTokens = JsonSerializer.Serialize(sessionTokens);

            await redisDatabase.StringSetAsync(
                KeyResolver.ResolveSessionKey(clientSessionKey),
                jsonTokens,
                _redisOptions.Value.SessionKeyTtl,
                When.Always
            );
        }
    }
}
