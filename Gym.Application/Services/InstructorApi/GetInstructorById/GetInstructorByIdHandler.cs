using Gym.Abstractions.Query.Instructors;
using MediatR;

namespace Gym.Application.Services.InstructorApi.GetInstructorById
{
    internal class GetInstructorByIdHandler(IInstructorProjectionQueryService _instructorProjectionQueryService) : IRequestHandler<GetInstructorById, InstructorProjection?>
    {
        public async Task<InstructorProjection?> Handle(GetInstructorById request, CancellationToken cancellationToken)
        {
            return await _instructorProjectionQueryService.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
