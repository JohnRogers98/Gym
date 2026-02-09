using MediatR;

namespace Gym.Application.Services.InstructorApi.CreateInstructor
{
    public record CreateInstructor(String FirstName, String LastName) : IRequest<InstructorDetails>;
}
