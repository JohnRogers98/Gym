using System.ComponentModel.DataAnnotations;

namespace Gym.WebApplication.Options
{
    public class BffOptions
    {
        public const String SectionName = "Bff";

        [Required]
        [Url]
        public String BaseUrl { get; set; } = default!;

        public String LoginEndpoint { get; set; } = "/login";
        public String FullLoginPath => $"{BaseUrl}{LoginEndpoint}";
    }
}
