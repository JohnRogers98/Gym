using Gym.Domain.InstructorContext;
using MediatR;

namespace Gym.Application.Services.InstructorApi.GetAllInstructors
{
    internal class GetAllInstructorsHandler(IInstructorQueryService _instructorQueryService) : IRequestHandler<GetAllInstructors, IEnumerable<InstructorDetails>>
    {
        public async Task<IEnumerable<InstructorDetails>> Handle(GetAllInstructors request, CancellationToken cancellationToken)
        {
            var instructors = await _instructorQueryService.GetAllAsync(cancellationToken);
            return instructors.Select(instructor => instructor.ToDetails());
        }
    }
}
