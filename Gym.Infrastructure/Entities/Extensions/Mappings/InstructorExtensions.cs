using Gym.Application.Extensions;
using Gym.Domain._Shared;
using Gym.Domain.InstructorContext;
using Gym.Domain.InstructorContext.ValueObjects;
using Gym.Infrastructure.Entities.Repositories.Instructors;

namespace Gym.Infrastructure.Entities.Extensions.Mappings
{
    internal static class InstructorExtensions
    {
        public static Instructor ToDomain(this InstructorEntity entity)
        {
            return Instructor.Restore(
                InstructorId.From(entity.Id.ToString()).Unwrap(),
                UserId.From(entity.UserId.ToString()).Unwrap()
            );
        }

        public static InstructorEntity ToEntity(this Instructor instructor)
        {
            return new() 
            { 
                Id = instructor.Id.Value.ToObjectId(),
                UserId = instructor.UserId.Value.ToObjectId()
            };
        }
    }
}
