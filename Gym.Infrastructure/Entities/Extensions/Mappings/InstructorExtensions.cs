using Gym.Domain.InstructorAggregate;
using Gym.Infrastructure.Entities.Repositories.Instructors;

namespace Gym.Infrastructure.Entities.Extensions.Mappings
{
    internal static class InstructorExtensions
    {
        public static Instructor ToDomain(this InstructorEntity entity)
        {
            return Instructor.Restore(InstructorId.From(entity.Id.ToString()), entity.FirstName, entity.LastName);
        }

        public static InstructorEntity ToEntity(this Instructor instructor)
        {
            return new() { Id = instructor.Id.Value.ToObjectId(), FirstName = instructor.FirstName, LastName = instructor.LastName };
        }
    }
}
