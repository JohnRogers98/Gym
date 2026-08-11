using System.ComponentModel.DataAnnotations;

namespace Gym.AuthorizationServer.Client.Options
{
    public class ClientCredentialsOptions
    {
        [Required]
        public String ClientId { get; set; } = default!;

        [Required]
        public String ClientSecret { get; set; } = default!;
        
        [Required]
        public String RedirectUri { get; set; } = default!;
        
        [Required]
        public String Scope { get; set; } = default!;
    }
}
