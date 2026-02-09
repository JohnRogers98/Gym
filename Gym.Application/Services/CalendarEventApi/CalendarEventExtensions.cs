using Gym.Application.Services.InstructorApi;
using Gym.Application.Services.TrainingApi;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.InstructorContext;
using Gym.Domain.TrainingContext;

namespace Gym.Application.Services.CalendarEventApi
{
    internal static class CalendarEventExtensions
    {
        public static CalendarEventDetails ToDetails(this CalendarEvent calendarEvent)
        {
            return new CalendarEventDetails(
                calendarEvent.Id.Value,
                calendarEvent.Start,
                calendarEvent.End,
                calendarEvent.Training.ToDetails(),
                calendarEvent.MaxClientCount,
                calendarEvent.Bookings.Select(aUserId => aUserId.Value),
                calendarEvent.Instructors.ToDetails());
        }

        public static TrainingDetails ToDetails(this TrainingInfo trainingInfo)
            => new TrainingDetails(trainingInfo.Id.Value, trainingInfo.Name, trainingInfo.Description);

        public static TrainingInfo ToInfo(this TrainingDetails trainingDetails)
            => TrainingInfo.Create(TrainingId.From(trainingDetails.Id), trainingDetails.Name, trainingDetails.Description);

        public static InstructorDetails ToDetails(this InstructorInfo instructorInfo)
            => new InstructorDetails(instructorInfo.Id.Value, instructorInfo.FirstName, instructorInfo.LastName);

        public static IEnumerable<InstructorDetails>? ToDetails(this IEnumerable<InstructorInfo>? instructorInfo)
            => instructorInfo?.Select(info => info.ToDetails());

        public static InstructorInfo ToInfo(this InstructorDetails instructorDetails)
            => InstructorInfo.Create(InstructorId.From(instructorDetails.Id), instructorDetails.FirstName, instructorDetails.LastName);

        public static IEnumerable<InstructorInfo>? ToInfos(this IEnumerable<InstructorDetails>? instructorDetails)
            => instructorDetails?.Select(details => details.ToInfo());
    }
}
