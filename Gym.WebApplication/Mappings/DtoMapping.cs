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
                  .ForMember(dest => dest.FullName, opt => opt.MapFrom((src, dest, _, context) => $"{src.FirstName} {src.LastName ?? String.Empty}"));

            base.CreateMap<AdminCalendarEventDto, CalendarItemViewModel>();
        }
    }
}
