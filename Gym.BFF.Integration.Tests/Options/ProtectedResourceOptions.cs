using System.ComponentModel.DataAnnotations;

namespace Gym.BFF.Integration.Tests.Options
{
    public class ProtectedResourceOptions
    {
        [Required]
        public String ClientName { get; set; } = default!;

        [Required]
        [Url]
        public String BaseUrl { get; set; } = default!;
    }
}
