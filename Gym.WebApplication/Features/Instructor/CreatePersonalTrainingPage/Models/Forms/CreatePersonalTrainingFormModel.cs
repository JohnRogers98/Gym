using Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Models.Forms;
using System.ComponentModel.DataAnnotations;

namespace Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Models.Forms
{
    public class CreatePersonalTrainingFormModel
    {
        [Required]
        public String? ClientId { get; set; }

        [Required, FromToday]
        public DateTime? LocalStartDateTime { get; set; }

        [Required]
        public TimeSpan? StartTimeSpan { get; set; }

        public Int32? DurationInMinutes { get; set; }

        public DateTime? LocalStart => LocalStartDateTime + StartTimeSpan;
        public DateTime? UtcStart => LocalStart?.ToUniversalTime();

        public DateTime? LocalEnd => DurationInMinutes.HasValue
            ? LocalStart + TimeSpan.FromMinutes(DurationInMinutes.Value)
            : null;
        public DateTime? UtcEnd => LocalEnd?.ToUniversalTime();

        public Boolean IsPaid { get; set; }

        public String? InstructorComment { get; set; }
    }
}
