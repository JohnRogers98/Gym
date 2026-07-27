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
            base.CreateMap<Abstractions.Query._CommonInfos.ClientInfo, WebDto.Responses.PersonalTraining.ClientInfo>();
            base.CreateMap<Abstractions.Query._CommonInfos.InstructorInfo, WebDto.Responses.PersonalTraining.InstructorInfo>();
        }
    }
}
