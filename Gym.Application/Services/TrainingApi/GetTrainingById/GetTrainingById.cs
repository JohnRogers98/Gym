using Gym.Abstractions.Query.Trainings;
using MediatR;

namespace Gym.Application.Services.TrainingApi.GetTrainingById
{
    public record GetTrainingById(String Id) : IRequest<TrainingProjection>;
}
