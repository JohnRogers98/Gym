using Gym.Application.Extensions;
using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.InstructorContext;
using MediatR;

namespace Gym.Application.Services.InstructorApi.CreateInstructor
{
    internal class CreateInstructorHandler(IInstructorRepository _instructorRepository) : IRequestHandler<CreateInstructor, Result<CreateInstructorResult>>
    {
        public async Task<Result<CreateInstructorResult>> Handle(CreateInstructor request, CancellationToken cancellationToken)
        {
            var shapeInstructorResult = from firstName in FirstName.From(request.FirstName)
                                        from lastName in LastName.From(request.LastName)
                                        select Instructor.Create(_instructorRepository.NextIdentity(), firstName, lastName);

            if (shapeInstructorResult.Success is false)
                return Result<CreateInstructorResult>.Fail(shapeInstructorResult.Error!);
            
            await _instructorRepository.SaveAsync(shapeInstructorResult.Data!, cancellationToken);

            return Result<CreateInstructorResult>.Ok(new CreateInstructorResult(shapeInstructorResult.Data!.Id.Value));
        }
    }
}
