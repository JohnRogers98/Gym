namespace Gym.Redis.Client
{
    internal static class KeyResolver
    {
        public static String ResolveSessionKey(String clientSessionKey)
        {
            return $"session:{clientSessionKey}";
        }
    }
}
