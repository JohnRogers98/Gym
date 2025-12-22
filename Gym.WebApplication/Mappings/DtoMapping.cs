using AutoMapper;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Dto;

namespace Gym.WebApplication.Mappings
{
    public class DtoMapping : Profile
    {
        public DtoMapping()
        {
            base.CreateMap<TrainingDto, TrainingViewModel>();

            base.CreateMap<InstructorDto, InstructorViewModel>()
                .ConstructUsing(src => new InstructorViewModel(src.id, $"{src.firstName} {src.lastName ?? String.Empty}"));

            base.CreateMap<CalendarEventDto, CalendarItemViewModel>();
        }
    }
}
