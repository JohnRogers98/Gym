using System.ComponentModel.DataAnnotations;

namespace Gym.BFF.Options
{
    public class CookieOptions
    {
        public const String SectionName = "Cookie";

        public TimeSpan Ttl { get; set; } = TimeSpan.FromHours(25);

        [Required(AllowEmptyStrings = false)]
        public required String Name { get; init; }

        [Required(AllowEmptyStrings = false)]
        public required String DataProtectorPurpose { get; init; }
    }
}
