using Gym.Domain.CalendarEventAggregate;
using Gym.Domain.InstructorAggregate;
using Gym.Domain.TrainingAggregate;
using Gym.Domain.UserAggregate;
using Gym.Infrastructure.Entities.Repositories.Instructors;
using Gym.Infrastructure.Entities.Repositories.Trainings;
using Gym.Infrastructure.Entities.Repositories.Users;
using MongoConsoleApp.Repositories.CalendarEvents;

namespace Gym.Infrastructure.Entities.Extensions
{
    internal static class MappingExtensions
    {
        public static User ToDomain(this UserEntity entity)
        {
            var isParsed = Enum.TryParse<UserRole>(entity.Role, true, out UserRole userRole);
            if (!isParsed)
            {
                throw new ArgumentException($"Failed to parse role for user {entity.Id}");
            }

            return User.Restore(UserId.From(entity.Id.ToString()), userRole, TelegramId.From(entity.TelegramId ?? default));
        }

        public static UserEntity ToEntity(this User user)
        {
            return new() { Id = user.Id.Value.ToObjectId(), Role = user.Role.ToString(), TelegramId = user.TelegramId?.Value };
        }

        public static Instructor ToDomain(this InstructorEntity entity)
        {
            return Instructor.Restore(InstructorId.From(entity.Id.ToString()), entity.FirstName, entity.LastName);
        }

        public static InstructorEntity ToEntity(this Instructor instructor)
        {
            return new() { Id = instructor.Id.Value.ToObjectId(), FirstName = instructor.FirstName, LastName = instructor.LastName };
        }


        public static Training ToDomain(this TrainingEntity entity)
        {
            return Training.Restore(TrainingId.From(entity.Id.ToString()), entity.Name, entity.Description);
        }

        public static TrainingEntity ToEntity(this Training training)
        {
            return new() { Id = training.Id.Value.ToObjectId(), Name = training.Name, Description = training.Description };
        }


        public static CalendarEvent ToDomain(this CalendarEventEntity entity)
        {
            TrainingInfo trainingInfo = TrainingInfo.Create(TrainingId.From(entity.Training.Id.ToString()), entity.Training.Name, entity.Training.Description);

            List<InstructorInfo> instructors = new();
            if (entity.Instructors is not null)
            {
                foreach (var anInstructorEntity in entity.Instructors)
                {
                    instructors.Add(InstructorInfo.Create(InstructorId.From(anInstructorEntity.Id.ToString()), anInstructorEntity.FirstName, anInstructorEntity.LastName));
                }
            }

            return CalendarEvent.Restore(CalendarEventId.From(entity.Id.ToString()), entity.Start, entity.End, trainingInfo, entity.MaxClientCount, instructors);
        }

        public static CalendarEventEntity ToEntity(this CalendarEvent calendarEvent)
        {
            TrainingEntity trainingEntity = new() 
            { 
                Id = calendarEvent.Training.Id.Value.ToObjectId(),
                Name = calendarEvent.Training.Name,
                Description = calendarEvent.Training.Description 
            };

            List<InstructorEntity> instructorEntities = new();
            if (calendarEvent.Instructors is not null)
            {
                foreach (var anInstructorInfo in calendarEvent.Instructors)
                {
                    instructorEntities.Add(new() { Id = anInstructorInfo.Id.Value.ToObjectId(), FirstName = anInstructorInfo.FirstName, LastName = anInstructorInfo.LastName });
                }
            }

            return new()
            {
                Id = calendarEvent.Id.Value.ToObjectId(),
                Start = calendarEvent.Start,
                End = calendarEvent.End,
                Training = trainingEntity,
                MaxClientCount = calendarEvent.MaxClientCount,
                Instructors = instructorEntities
            };
        }

    }
}
