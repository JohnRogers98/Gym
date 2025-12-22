using MediatR;

namespace Gym.Application.Services.TrainingApi.GetAllTrainings
{
    public record GetAllTrainingsQuery : IRequest<IEnumerable<TrainingDetails>>;
}
