using MediatR;

namespace Gym.Application.Services.TrainingApi.GetTrainingById
{
    public record GetTrainingById(String Id) : IRequest<TrainingDetails>;
}
