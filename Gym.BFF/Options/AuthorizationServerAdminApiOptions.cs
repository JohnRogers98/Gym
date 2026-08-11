using System.ComponentModel.DataAnnotations;

namespace Gym.BFF.Options
{
    public class AuthorizationServerAdminApiOptions
    {
        public const String SectionName = "Urls:AuthorizationServerAdminApi";

        public String ClientName { get; set; } = "auth-server-admin";

        [Required]
        [Url]
        public String BaseUrl { get; set; } = default!;
    }
}
