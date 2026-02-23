using Gym.Abstractions.Query.Instructors;
using MediatR;

namespace Gym.Application.Services.InstructorApi.GetAllInstructors
{
    internal class GetAllInstructorsHandler(IInstructorProjectionQueryService _instructorProjectionQueryService) : IRequestHandler<GetAllInstructors, IEnumerable<InstructorProjection>>
    {
        public async Task<IEnumerable<InstructorProjection>> Handle(GetAllInstructors request, CancellationToken cancellationToken)
        {
            return await _instructorProjectionQueryService.GetAllAsync(cancellationToken);
        }
    }
}
