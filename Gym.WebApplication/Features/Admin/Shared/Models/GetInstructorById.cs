using Gym.WebApplication.Features.Admin.Shared.ValueObjects;

namespace Gym.WebApplication.Features.Admin.Shared.Models
{
    public class GetInstructorById
    {
        public required InstructorId InstructorId { get; set; }
    }
}
