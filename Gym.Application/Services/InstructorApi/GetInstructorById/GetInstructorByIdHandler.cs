using Gym.Application.Extensions;
using Gym.Domain.InstructorAggregate;
using MediatR;

namespace Gym.Application.Services.InstructorApi.GetInstructorById
{
    internal class GetInstructorByIdHandler(IInstructorQueryService _instructorQueryService) : IRequestHandler<GetInstructorByIdQuery, InstructorDetails>
    {
        public async Task<InstructorDetails> Handle(GetInstructorByIdQuery request, CancellationToken cancellationToken)
        {
            Instructor? instructor = await _instructorQueryService.GetByIdAsync(InstructorId.From(request.id), cancellationToken);

            if (instructor is null) throw new ArgumentException();

            return instructor.ToDetails();
        }
    }
}
