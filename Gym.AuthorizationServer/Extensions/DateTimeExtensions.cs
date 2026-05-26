namespace Gym.AuthorizationServer.Extensions
{
    public static class DateTimeExtensions
    {
        public static Int32 GetSecondsFromUtcNow(this DateTime dateTime)
            => (Int32)(dateTime - DateTime.UtcNow).TotalSeconds;
    }
}
