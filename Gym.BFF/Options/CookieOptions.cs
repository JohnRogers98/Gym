namespace Gym.BFF.Options
{
    public class CookieOptions
    {
        public const String SectionName = "Cookie";

        public TimeSpan Ttl { get; set; } = TimeSpan.FromHours(25);
    }
}
