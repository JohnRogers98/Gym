using MediatR;

namespace Gym.Application.Services.InstructorApi.GetAllInstructors
{
    public record GetAllInstructors : IRequest<IEnumerable<InstructorDetails>>;
}
