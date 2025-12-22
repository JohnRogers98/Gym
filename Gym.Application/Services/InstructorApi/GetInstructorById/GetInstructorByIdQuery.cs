using MediatR;

namespace Gym.Application.Services.InstructorApi.GetInstructorById
{
    public record GetInstructorByIdQuery(String id) : IRequest<InstructorDetails>;
}
