using MediatR;

namespace Gym.Application.Services.TrainingApi.CreateTraining
{
    public record CreateTraining(String Name, String Description) : IRequest<TrainingDetails>;
}
