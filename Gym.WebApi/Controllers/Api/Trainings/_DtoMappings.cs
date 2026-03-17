using AutoMapper;
using Gym.Abstractions.Query.Trainings;
using Gym.Application.Services.TrainingApi.CreateTraining;
using Gym.WebDto.Requests.Training;
using Gym.WebDto.Responses.Training;

namespace Gym.WebApi.Controllers.Api.Trainings
{
    public class _DtoMappings : Profile
    {
        public _DtoMappings()
        {
            CreateMap<CreateTrainingRequest, CreateTraining>();
            CreateMap<CreateTrainingResult, CreateTrainingResponse>();

            CreateMap<TrainingProjection, TrainingDto>();
        }
    }
}
