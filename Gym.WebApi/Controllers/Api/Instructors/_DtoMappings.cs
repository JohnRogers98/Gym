using AutoMapper;
using Gym.Abstractions.Query.Instructors;
using Gym.WebDto.Responses.Instructor;

namespace Gym.WebApi.Controllers.Api.Instructors
{
    public class _DtoMappings : Profile
    {
        public _DtoMappings()
        {
            CreateMap<InstructorProjection, InstructorDto>();
        }
    }
}
