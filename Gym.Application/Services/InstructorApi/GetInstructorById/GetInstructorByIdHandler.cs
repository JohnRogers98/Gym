using Gym.Domain.InstructorContext;
using MediatR;

namespace Gym.Application.Services.InstructorApi.GetInstructorById
{
    internal class GetInstructorByIdHandler(IInstructorQueryService _instructorQueryService) : IRequestHandler<GetInstructorById, InstructorDetails>
    {
        public async Task<InstructorDetails> Handle(GetInstructorById request, CancellationToken cancellationToken)
        {
            Instructor? instructor = await _instructorQueryService.GetByIdAsync(InstructorId.From(request.Id), cancellationToken);

            if (instructor is null) throw new ArgumentException();

            return instructor.ToDetails();
        }
    }
}
