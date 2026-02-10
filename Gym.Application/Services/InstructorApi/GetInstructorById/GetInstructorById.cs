using MediatR;

namespace Gym.Application.Services.InstructorApi.GetInstructorById
{
    public record GetInstructorById(String Id) : IRequest<InstructorDetails>;
}
