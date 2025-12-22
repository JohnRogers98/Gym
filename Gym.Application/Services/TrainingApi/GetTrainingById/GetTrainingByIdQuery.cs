using MediatR;

namespace Gym.Application.Services.TrainingApi.GetTrainingById
{
    public record GetTrainingByIdQuery(String id) : IRequest<TrainingDetails>;
}
