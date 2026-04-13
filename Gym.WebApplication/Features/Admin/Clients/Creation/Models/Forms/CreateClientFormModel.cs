using System.ComponentModel.DataAnnotations;

namespace Gym.WebApplication.Features.Admin.Clients.Creation.Models.Forms
{
    public class CreateClientFormModel
    {
        [Required]
        public String? Login { get; set; }

        [Required]
        public String? FirstName { get; set; }

        public String? LastName { get; set; }
    }
}
