using System.ComponentModel.DataAnnotations;

namespace Gym.WebApplication.Features.Admin.Trainings.Creation.Models.Forms
{
    public class CreateTrainingFormModel
    {
        [Required]
        public String? Name { get; set; }

        public String? Description { get; set; }
    }
}
