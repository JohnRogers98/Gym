using Gym.Application.Services.InstructorApi;
using Gym.Application.Services.TrainingApi;

namespace Gym.Application.Services.CalendarEventApi
{
    public record CalendarEventDetails(
        String id,
        DateTime start,
        DateTime? end,
        TrainingDetails training,
        Int32? maxClientCount,
        IEnumerable<InstructorDetails>? instructors);
}
