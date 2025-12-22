using Gym.Application.Extensions;
using Gym.Domain.InstructorAggregate;
using MediatR;

namespace Gym.Application.Services.InstructorApi.GetAllInstructors
{
    internal class GetAllInstructorsHandler(IInstructorQueryService _instructorQueryService) : IRequestHandler<GetAllInstructorsQuery, IEnumerable<InstructorDetails>>
    {
        public async Task<IEnumerable<InstructorDetails>> Handle(GetAllInstructorsQuery request, CancellationToken cancellationToken)
        {
            var instructors = await _instructorQueryService.GetAllAsync(cancellationToken);
            return instructors.Select(instructor => instructor.ToDetails());
        }
    }
}
