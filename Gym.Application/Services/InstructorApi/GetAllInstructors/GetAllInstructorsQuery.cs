using Gym.Domain.InstructorAggregate;
using MediatR;

namespace Gym.Application.Services.InstructorApi.GetAllInstructors
{
    public record GetAllInstructorsQuery : IRequest<IEnumerable<InstructorDetails>>;
}
