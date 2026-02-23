using AutoMapper;
using Gym.Abstractions.Query.Instructors;
using Gym.Application.Services.InstructorApi.CreateInstructor;
using Gym.WebDto.Requests.Instructor;
using Gym.WebDto.Responses.Instructor;

namespace Gym.WebApi.Controllers.Api.Instructors
{
    public class _DtoMappings : Profile
    {
        public _DtoMappings()
        {
            CreateMap<CreateInstructorRequest, CreateInstructor>();
            CreateMap<CreateInstructorResult, CreateInstructorResponse>();

            CreateMap<InstructorProjection, GetInstructorResponse>();
            CreateMap<InstructorProjection, InstructorDto>();
        }
    }
}
