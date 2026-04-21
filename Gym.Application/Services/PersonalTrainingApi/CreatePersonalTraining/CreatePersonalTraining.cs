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
        Boolean IsPaid,
        String InstructorComment) : IRequest<Result<CreatePersonalTrainingResult>>, ITransactionalRequest;
}
