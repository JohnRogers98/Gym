using System.ComponentModel.DataAnnotations;

namespace Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Models.Forms
{
    public class CreatePollFormModel
    {
        [Required]
        public String? Title { get; set; }

        public Boolean IsRequired { get; set; }

        public Boolean CanSelectMany { get; set; }

        [MinItems(1)]
        public List<String> Choices { get; set; } = [];
    }
}
