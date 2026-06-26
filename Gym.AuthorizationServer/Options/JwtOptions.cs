using System.ComponentModel.DataAnnotations;

namespace Gym.AuthorizationServer.Options
{
    public class JwtOptions
    {
        public const String SectionName = "Jwt";

        [Required]
        [Url]
        public String Issuer { get; set; } = default!;

        [Required]
        public String RsaKeyId { get; set; } = default!;

        [Required]
        public String RsaKeyPath { get; set; } = default!;
    }
}
