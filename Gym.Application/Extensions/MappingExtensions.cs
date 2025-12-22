using Gym.Application.Services.CalendarEventApi;
using Gym.Application.Services.InstructorApi;
using Gym.Application.Services.TrainingApi;
using Gym.Domain.CalendarEventAggregate;
using Gym.Domain.InstructorAggregate;
using Gym.Domain.TrainingAggregate;

namespace Gym.Application.Extensions
{
    public static class MappingExtensions
    {
        public static CalendarEventDetails ToDetails(this CalendarEvent calendarEvent) 
        {
            return new CalendarEventDetails(
                calendarEvent.Id.Value,
                calendarEvent.Start,
                calendarEvent.End,
                calendarEvent.Training.ToDetails(),
                calendarEvent.MaxClientCount,
                calendarEvent.Instructors.ToDetails());
        }

        public static TrainingDetails ToDetails(this TrainingInfo trainingInfo)
            => new TrainingDetails(trainingInfo.Id.Value, trainingInfo.Name, trainingInfo.Description);

        public static TrainingInfo ToInfo(this TrainingDetails trainingDetails)
            => TrainingInfo.Create(TrainingId.From(trainingDetails.id), trainingDetails.name, trainingDetails.description);

        public static InstructorDetails ToDetails(this InstructorInfo instructorInfo)
            => new InstructorDetails(instructorInfo.Id.Value, instructorInfo.FirstName, instructorInfo.LastName);

        public static IEnumerable<InstructorDetails>? ToDetails(this IEnumerable<InstructorInfo>? instructorInfo)
            => instructorInfo?.Select(info => info.ToDetails());

        public static InstructorInfo ToInfo(this InstructorDetails instructorDetails)
            => InstructorInfo.Create(InstructorId.From(instructorDetails.id), instructorDetails.firstName, instructorDetails.lastName);

        public static IEnumerable<InstructorInfo>? ToInfos(this IEnumerable<InstructorDetails>? instructorDetails)
            => instructorDetails?.Select(details => details.ToInfo());

        public static InstructorDetails ToDetails(this Instructor instructor)
            => new InstructorDetails(instructor.Id.Value, instructor.FirstName, instructor.LastName);

        public static TrainingDetails ToDetails(this Training training)
            => new TrainingDetails(training.Id.Value, training.Name, training.Description);
    }
}
