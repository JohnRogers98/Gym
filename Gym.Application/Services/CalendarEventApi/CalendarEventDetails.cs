using Gym.Application.Services.InstructorApi;
using Gym.Application.Services.TrainingApi;

namespace Gym.Application.Services.CalendarEventApi
{
    public record CalendarEventDetails(
        String Id,
        DateTime Start,
        DateTime? End,
        TrainingDetails Training,
        Int32? MaxClientCount,
        IEnumerable<String> BookingUsers,
        IEnumerable<InstructorDetails>? Instructors);
}
