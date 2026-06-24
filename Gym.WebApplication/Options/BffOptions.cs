using System.ComponentModel.DataAnnotations;

namespace Gym.WebApplication.Options
{
    public class BffOptions
    {
        public const String SectionName = "Bff";

        [Required]
        [Url]
        public String BaseUrl { get; set; } = default!;

        public String ClientName { get; set; } = "bff-client";

        public String LoginEndpoint { get; set; } = "/login";
        public String FullLoginPath => $"{BaseUrl}{LoginEndpoint}";

        public String TelegramInitEndpoint { get; set; } = "/telegram-init";
        public String FullTelegramInitPath => $"{BaseUrl}{TelegramInitEndpoint}";

        public String CheckSessionEndpoint { get; set; } = "/check-session";
        public String FullCheckSessionPath => $"{BaseUrl}{CheckSessionEndpoint}";

        public String UserInfoEndpoint { get; set; } = "/api/userinfo";
        public String FullUserInfoPath => $"{BaseUrl}{UserInfoEndpoint}";
    }
}
