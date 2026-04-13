using System.ComponentModel.DataAnnotations;

namespace Gym.WebApplication.Features.Login.FormAuth.Models.Forms
{
    public class AuthFormModel
    {
        [Required]
        public String? Login { get; set; }

        [Required]
        public String? Password { get; set; }
    }
}
