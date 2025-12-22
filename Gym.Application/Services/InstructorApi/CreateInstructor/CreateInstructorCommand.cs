using MediatR;

namespace Gym.Application.Services.InstructorApi.CreateInstructor
{
    public record CreateInstructorCommand(String firstName, String lastName) : IRequest<InstructorDetails>;
}
