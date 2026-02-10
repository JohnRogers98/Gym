using Gym.Domain.InstructorContext;

namespace Gym.Application.Services.InstructorApi
{
    internal static class InstructorExtensions
    {
        public static InstructorDetails ToDetails(this Instructor instructor)
            => new InstructorDetails(instructor.Id.Value, instructor.FirstName, instructor.LastName);
    }
}
