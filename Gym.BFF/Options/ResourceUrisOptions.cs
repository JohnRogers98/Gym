using System.ComponentModel.DataAnnotations;

namespace Gym.BFF.Options
{
    public class ResourceUrisOptions
    {
        public const String SectionName = "ResourceUris";

        [Required]
        public String Api { get; set; } = default!;
    }
}
