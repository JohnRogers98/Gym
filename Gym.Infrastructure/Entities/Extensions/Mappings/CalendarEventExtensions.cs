using Gym.Application.Extensions;
using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.CalendarEventContext.ValueObjects;
using Gym.Domain.InstructorContext.ValueObjects;
using Gym.Domain.TrainingContext.ValueObjects;
using Gym.Infrastructure.Entities.Repositories.CalendarEvents;

namespace Gym.Infrastructure.Entities.Extensions.Mappings
{
    internal static class CalendarEventExtensions
    {
        public static CalendarEvent ToDomain(this CalendarEventEntity entity)
        {
            return CalendarEvent.Restore(
                CalendarEventId.From(entity.Id.ToString()).Unwrap(),
                TrainingPeriod.From(
                    StartsAt.From(entity.Start).Unwrap(),
                    entity.End.HasValue ? EndsAt.From(entity.End.Value).Unwrap() : null
                ).Unwrap(),
                Enum.Parse<CalendarEventStatus>(entity.Status),
                entity.MaxClientCount.HasValue ? Capacity.From(entity.MaxClientCount.Value).Unwrap() : Capacity.Unlimited(), 
                TrainingId.From(entity.TrainingId.ToString()).Unwrap(),
                entity.Bookings?.Select(x => UserId.From(x.ToString()).Unwrap()) ?? new HashSet<UserId>(),
                entity.Instructors?.Select(instructorId => InstructorId.From(instructorId.ToString()).Unwrap())
            );
        }

        public static CalendarEventEntity ToEntity(this CalendarEvent calendarEvent)
        {
            return new CalendarEventEntity()
            {
                Id = calendarEvent.Id.Value.ToObjectId(),
                Start = calendarEvent.TrainingPeriod.StartsAt.Value,
                End = calendarEvent.TrainingPeriod.EndsAt?.Value,
                Status = calendarEvent.Status.ToString(),
                TrainingId = calendarEvent.TrainingId.Value.ToObjectId(),
                Bookings = calendarEvent.Bookings.Select(anUserId => anUserId.Value.ToObjectId()),
                MaxClientCount = calendarEvent.Capacity.Value,
                Instructors = calendarEvent.Instructors?.Select(instructorId => instructorId.Value.ToObjectId())
            };
        }
    }
}
