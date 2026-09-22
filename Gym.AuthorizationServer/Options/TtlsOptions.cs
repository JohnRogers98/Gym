namespace Gym.AuthorizationServer.Options
{
    public class TtlsOptions
    {
        public const String SectionName = "Ttls";

        public TimeSpan AccessToken { get; set; } = TimeSpan.FromMinutes(5);
        public TimeSpan RefreshToken { get; set; } = TimeSpan.FromDays(1);
        public TimeSpan IdToken { get; set; } = TimeSpan.FromMinutes(2);
    }
}
