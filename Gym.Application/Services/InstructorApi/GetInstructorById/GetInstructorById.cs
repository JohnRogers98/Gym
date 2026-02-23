using Gym.Abstractions.Query.Instructors;
using MediatR;

namespace Gym.Application.Services.InstructorApi.GetInstructorById
{
    public record GetInstructorById(String Id) : IRequest<InstructorProjection>;
}
