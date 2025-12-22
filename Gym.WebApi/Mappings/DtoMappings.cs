using AutoMapper;
using Gym.Application.Services.CalendarEventApi;
using Gym.Application.Services.CalendarEventApi.CreateCalendarEvent;
using Gym.Application.Services.InstructorApi;
using Gym.Application.Services.InstructorApi.CreateInstructor;
using Gym.Application.Services.TrainingApi;
using Gym.Application.Services.TrainingApi.CreateTraining;
using Gym.WebDto.Dto;
using Gym.WebDto.Requests.CalendarEvent;
using Gym.WebDto.Requests.Instructor;
using Gym.WebDto.Requests.Training;
using Gym.WebDto.Responses.CalendarEvent;
using Gym.WebDto.Responses.Instructor;
using Gym.WebDto.Responses.Training;

namespace Gym.WebApi.Mappings
{
    public class DtoMappings : Profile
    {
        public DtoMappings()
        {
            CreateMap<CreateInstructorRequest, CreateInstructorCommand>();
            CreateMap<InstructorDetails, CreateInstructorResponse>();

            CreateMap<InstructorDetails, GetInstructorResponse>();

            CreateMap<CreateTrainingRequest, CreateTrainingCommand>();
            CreateMap<TrainingDetails, CreateTrainingResponse>();

            CreateMap<TrainingDetails, GetTrainingResponse>();

            CreateMap<InstructorDetails, InstructorDto>();
            CreateMap<InstructorDto, InstructorDetails>();
            CreateMap<TrainingDetails, TrainingDto>();
            CreateMap<TrainingDto, TrainingDetails>();

            CreateMap<CreateCalendarEventRequest, CreateCalendarEventCommand>();
            CreateMap<CalendarEventDetails, CreateCallendarEventResponse>();

            CreateMap<CalendarEventDetails, GetCalendarEventResponse>();
            CreateMap<CalendarEventDetails, CalendarEventDto>();
        }
    }
}
