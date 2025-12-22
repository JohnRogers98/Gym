using MediatR;

namespace Gym.Application.Services.TrainingApi.CreateTraining
{
    public record CreateTrainingCommand(String name, String description) : IRequest<TrainingDetails>;
}
