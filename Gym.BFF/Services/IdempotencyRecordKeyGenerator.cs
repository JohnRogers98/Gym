namespace Gym.BFF.Services
{
    public interface IIdempotencyRecordKeyGenerator
    {
        String Generate(String clientSessionKey, String idempotencyKey);
    }

    public class IdempotencyRecordKeyGenerator : IIdempotencyRecordKeyGenerator
    {
        public String Generate(String clientSessionKey, String idempotencyKey)
        {
            return $"{clientSessionKey}:{idempotencyKey}";
        }
    }
}
