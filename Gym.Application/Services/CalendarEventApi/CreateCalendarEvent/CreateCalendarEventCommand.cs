using Gym.Application.Services.InstructorApi;
using Gym.Application.Services.TrainingApi;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.CreateCalendarEvent
{
    public record CreateCalendarEventCommand(
        DateTime start,
        DateTime end,
        Int32 maxClientCount,
        TrainingDetails training,
        IEnumerable<InstructorDetails> instructors) : IRequest<CalendarEventDetails>;
}
