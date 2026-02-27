using AutoMapper;
using Gym.WebApplication.Features.Admin.Instructors.Registration.Models.Forms;
using Gym.WebApplication.Features.Admin.Instructors.Registration.Models.Results;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Requests.Instructor;
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
            base.CreateMap<TrainingInfo, TrainingViewModel>();

            base.CreateMap<InstructorDto, InstructorViewModel>();
            base.CreateMap<InstructorInfo, InstructorViewModel>();

            base.CreateMap<InstructorRegistrationFormModel, CreateInstructorRequest>();
            base.CreateMap<CreateInstructorResponse, CreateInstructorResult>();

            base.CreateMap<GetInstructorResponse, InstructorViewModel>();

            base.CreateMap<ClientCalendarEventDto, CalendarItemViewModel>();
        }
    }
}
