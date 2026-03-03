using System.ComponentModel.DataAnnotations;

namespace Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Models.Forms
{
    public class CreateCalendarEventFormModel
    {
        [Required, FromToday]
        public DateTime? StartDateTime { get; set; }

        [Required]
        public TimeSpan? StartTimeSpan { get; set; }

        public Int32? DurationInMinutes { get; set; }

        public DateTime? Start => StartDateTime + StartTimeSpan;
        public DateTime? End => DurationInMinutes.HasValue
            ? Start + TimeSpan.FromMinutes(DurationInMinutes.Value)
            : null;

        [Required]
        public String? TrainingId { get; set; }

        public Int32? MaxClientCount { get; set; }

        public IEnumerable<String> Instructors { get; set; } = [];
    }
}
