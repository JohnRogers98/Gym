using System.ComponentModel.DataAnnotations;

namespace Gym.WebApplication.Features.Admin.Clients.TableView.Models.Forms
{
    public class ChargeClientFormModel
    {
        [Required]
        public String? ClientId { get; set; }

        [Required, Range(1, 100)]
        public Int32 ByCount { get; set; }
    }
}
