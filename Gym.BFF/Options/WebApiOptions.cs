using System.ComponentModel.DataAnnotations;

namespace Gym.BFF.Options
{
    public class WebApiOptions
    {
        public const String SectionName = "Urls:WebApi";

        [Required]
        public String ClientName { get; set; } = default!;

        [Required]
        [Url]
        public String BaseUrl { get; set; } = default!;
    }
}
