using System.ComponentModel.DataAnnotations;

namespace Gym.WebApplication.Features.Admin.Instructors.Registration.Models.Forms
{
    public class InstructorRegistrationFormModel
    {
        [Required]
        public String? FirstName { get; set; }

        [Required]
        public String? LastName { get; set; }
    }
}
