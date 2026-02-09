using MediatR;

namespace Gym.Application.Services.TrainingApi.GetAllTrainings
{
    public record GetAllTrainings : IRequest<IEnumerable<TrainingDetails>>;
}
