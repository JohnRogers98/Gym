using System.ComponentModel.DataAnnotations;

namespace Gym.BFF.Options
{
    public class ClientCredentialsOptions
    {
        public const String SectionName = "ClientCredentials";

        [ConfigurationKeyName("ClientId")]
        [Required(ErrorMessage = $"{nameof(ClientId)} is required")]
        public String ClientId { get; set; } = default!;

        [ConfigurationKeyName("ClientSecret")]
        [Required(ErrorMessage = $"{nameof(ClientSecret)} is required")]
        public String ClientSecret { get; set; } = default!;

        [ConfigurationKeyName("RedirectUri")]
        [Required(ErrorMessage = $"{nameof(RedirectUri)} is required")]
        public String RedirectUri { get; set; } = default!;

        [ConfigurationKeyName("Scope")]
        [Required(ErrorMessage = $"{nameof(Scope)} is required")]
        public String Scope { get; set; } = default!;
    }
}
