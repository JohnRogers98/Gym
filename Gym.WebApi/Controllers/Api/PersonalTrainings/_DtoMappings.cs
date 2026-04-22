using AutoMapper;
using Gym.Abstractions.Query.PersonalTrainings;
using Gym.WebDto.Responses.PersonalTraining;

namespace Gym.WebApi.Controllers.Api.PersonalTrainings
{
    public class _DtoMappings : Profile
    {
        public _DtoMappings() 
        {
            base.CreateMap<PersonalTrainingProjection, PersonalTrainingDto>();
        }
    }
}
