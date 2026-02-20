using Gym.Abstractions.Query.Trainings;
using MediatR;

namespace Gym.Application.Services.TrainingApi.GetAllTrainings
{
    public record GetAllTrainings : IRequest<IEnumerable<TrainingProjection>>;
}
