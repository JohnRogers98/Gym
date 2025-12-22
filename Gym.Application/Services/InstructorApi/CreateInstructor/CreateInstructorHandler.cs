using Gym.Application.Extensions;
using Gym.Domain.InstructorAggregate;
using MediatR;

namespace Gym.Application.Services.InstructorApi.CreateInstructor
{
    internal class CreateInstructorHandler(IInstructorRepository _instructorRepository) : IRequestHandler<CreateInstructorCommand, InstructorDetails>
    {
        public async Task<InstructorDetails> Handle(CreateInstructorCommand request, CancellationToken cancellationToken)
        {
            Instructor instructor = Instructor.Create(_instructorRepository.NextIdentity(), request.firstName, request.lastName); 
            await _instructorRepository.SaveAsync(instructor, cancellationToken);

            return instructor.ToDetails();
        }
    }
}
