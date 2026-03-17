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
                FirstName.From(entity.FirstName).Unwrap(),
                entity.LastName is not null ? LastName.From(entity.LastName).Unwrap() : null 
            );
        }

        public static InstructorEntity ToEntity(this Instructor instructor)
        {
            return new() 
            { 
                Id = instructor.Id.Value.ToObjectId(),
                FirstName = instructor.FirstName.Value,
                LastName = instructor.LastName?.Value 
            };
        }
    }
}
