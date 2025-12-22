using Gym.Domain.InstructorAggregate;

namespace Gym.Domain.CalendarEventAggregate
{
    public record InstructorInfo
    {
        public InstructorId Id { get; }
        public String FirstName { get; }
        public String? LastName { get; }

        private InstructorInfo(InstructorId id, String firstName, String? lastName) 
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public static InstructorInfo From(Instructor instructor) 
            => new InstructorInfo(instructor.Id, instructor.FirstName, instructor.LastName);

        public static InstructorInfo Create(InstructorId id, String firstName, String? lastName)
            => new InstructorInfo(id, firstName, lastName);
    }
}
