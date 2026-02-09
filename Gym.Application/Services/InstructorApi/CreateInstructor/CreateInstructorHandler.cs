using Gym.Domain.InstructorContext;
using MediatR;

namespace Gym.Application.Services.InstructorApi.CreateInstructor
{
    internal class CreateInstructorHandler(IInstructorRepository _instructorRepository) : IRequestHandler<CreateInstructor, InstructorDetails>
    {
        public async Task<InstructorDetails> Handle(CreateInstructor request, CancellationToken cancellationToken)
        {
            Instructor instructor = Instructor.Create(_instructorRepository.NextIdentity(), request.FirstName, request.LastName); 
            await _instructorRepository.SaveAsync(instructor, cancellationToken);

            return instructor.ToDetails();
        }
    }
}
