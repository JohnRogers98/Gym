using Gym.Application.Aspects;
using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Services.TrainingApi.CreateTraining
{
    public record CreateTraining(String Name, String Description) : IRequest<Result<CreateTrainingResult>>, ITransactionalRequest;
}
