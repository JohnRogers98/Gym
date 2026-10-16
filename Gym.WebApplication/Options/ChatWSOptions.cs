using System.ComponentModel.DataAnnotations;

namespace Gym.WebApplication.Options
{
    public class ChatWSOptions
    {
        public const String SectionName = "ChatWS";

        [Required]
        [Url]
        public String Url { get; set; } = default!;
    }
}
