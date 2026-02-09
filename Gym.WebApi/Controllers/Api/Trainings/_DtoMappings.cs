using AutoMapper;
using Gym.Application.Services.TrainingApi;
using Gym.Application.Services.TrainingApi.CreateTraining;
using Gym.WebDto.Dto;
using Gym.WebDto.Requests.Training;
using Gym.WebDto.Responses.Training;

namespace Gym.WebApi.Controllers.Api.Trainings
{
    public class _DtoMappings : Profile
    {
        public _DtoMappings()
        {
            CreateMap<CreateTrainingRequest, CreateTraining>();
            CreateMap<TrainingDetails, CreateTrainingResponse>();
            CreateMap<TrainingDetails, GetTrainingResponse>();
            CreateMap<TrainingDetails, TrainingDto>();
            CreateMap<TrainingDto, TrainingDetails>();
        }
    }
}
