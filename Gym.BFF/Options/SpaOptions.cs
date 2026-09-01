using System.ComponentModel.DataAnnotations;

namespace Gym.BFF.Options
{
    public class SpaOptions
    {
        public const String SectionName = "Urls:Spa";

        [Required]
        [Url]
        public String BaseUrl { get; set; } = default!;

        [Required]
        public String CallbackEndpoint { get; set; } = default!;
    }
}
