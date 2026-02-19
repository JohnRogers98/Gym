using AutoMapper;
using Gym.Application.Services.InstructorApi;
using Gym.WebDto.Requests.Instructor;
using Gym.WebDto.Responses.Instructor;

namespace Gym.WebApi.Controllers.Api.Instructors
{
    public class _DtoMappings : Profile
    {
        public _DtoMappings()
        {
            CreateMap<CreateInstructorRequest, CreateInstructorRequest>();
            CreateMap<InstructorDetails, CreateInstructorResponse>();
            CreateMap<InstructorDetails, GetInstructorResponse>();
            CreateMap<InstructorDetails, InstructorDto>();
            CreateMap<InstructorDto, InstructorDetails>();
        }
    }
}
