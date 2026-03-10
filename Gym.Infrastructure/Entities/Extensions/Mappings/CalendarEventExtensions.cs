using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.InstructorContext;
using Gym.Domain.TrainingContext;
using Gym.Infrastructure.Entities.Repositories.CalendarEvents;

namespace Gym.Infrastructure.Entities.Extensions.Mappings
{
    internal static class CalendarEventExtensions
    {
        public static CalendarEvent ToDomain(this CalendarEventEntity entity)
        {
            return CalendarEvent.Restore(
                CalendarEventId.From(entity.Id.ToString()),
                entity.Start,
                entity.End,
                Enum.Parse<CalendarEventStatus>(entity.Status),
                TrainingId.From(entity.TrainingId.ToString()),
                entity.Bookings?.Select(x => UserId.From(x.ToString())) ?? new HashSet<UserId>(),
                entity.MaxClientCount,
                entity.Instructors?.Select(instructorId => InstructorId.From(instructorId.ToString())));
        }

        public static CalendarEventEntity ToEntity(this CalendarEvent calendarEvent)
        {
            return new CalendarEventEntity()
            {
                Id = calendarEvent.Id.Value.ToObjectId(),
                Start = calendarEvent.Start,
                End = calendarEvent.End,
                Status = calendarEvent.Status.ToString(),
                TrainingId = calendarEvent.TrainingId.Value.ToObjectId(),
                Bookings = calendarEvent.Bookings.Select(anUserId => anUserId.Value.ToObjectId()),
                MaxClientCount = calendarEvent.MaxClientCount,
                Instructors = calendarEvent.Instructors?.Select(instructorId => instructorId.Value.ToObjectId())
            };
        }
    }
}
