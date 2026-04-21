using Gym.Application.Aspects;
using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Services.PersonalTrainingApi.CreatePersonalTraining
{
    public record CreatePersonalTraining(
        String InstructorId,
        String ClientId,
        DateTime Start,
        DateTime? End,
        String InstructorComment, 
        Boolean IsPaid) : IRequest<Result<CreatePersonalTrainingResult>>, ITransactionalRequest;
}
