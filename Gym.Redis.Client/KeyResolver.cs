namespace Gym.Redis.Client
{
    internal static class KeyResolver
    {
        public static String ResolveSessionKey(String clientSessionKey)
        {
            return $"session:{clientSessionKey}";
        }
        public static String ResolveIdempotencyRecordKey(String idempotencyRecordKey)
        {
            return $"idempotency:{idempotencyRecordKey}";
        }
    }
}
