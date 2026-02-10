using Gym.Application.Services.InstructorApi;
using Gym.Application.Services.TrainingApi;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.CreateCalendarEvent
{
    public record CreateCalendarEvent(
        DateTime Start,
        DateTime End,
        Int32 MaxClientCount,
        TrainingDetails Training,
        IEnumerable<InstructorDetails> Instructors) : IRequest<CalendarEventDetails>;
}
