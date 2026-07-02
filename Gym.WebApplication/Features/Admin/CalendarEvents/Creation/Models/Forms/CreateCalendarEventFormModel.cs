using System.ComponentModel.DataAnnotations;

namespace Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Models.Forms
{
    public class CreateCalendarEventFormModel
    {
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

        [Required]
        public String? TrainingId { get; set; }

        public Int32? MaxClientCount { get; set; }

        public IReadOnlyCollection<String> Instructors { get; set; } = [];

        public CreatePollFormModel? PollFormModel { get; set;  }
    }
}
