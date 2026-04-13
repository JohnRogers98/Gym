using System.ComponentModel.DataAnnotations;

namespace Gym.WebApplication.Features.Account.ChangePassword.Models.Forms
{
    public class ChangePasswordFormModel
    {
        [Required]
        public String? OldPassword { get; set; }

        [Required]
        public String? NewPassword { get; set; } 
    }
}
