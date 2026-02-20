using AutoMapper;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses.CalendarEvent;
using Gym.WebDto.Responses.Instructor;
using Gym.WebDto.Responses.Training;

namespace Gym.WebApplication.Mappings
{
    public class DtoMapping : Profile
    {
        public DtoMapping()
        {
            base.CreateMap<TrainingDto, TrainingViewModel>();

            base.CreateMap<InstructorDto, InstructorViewModel>();

            base.CreateMap<ClientCalendarEventDto, CalendarItemViewModel>();
        }
    }
}
