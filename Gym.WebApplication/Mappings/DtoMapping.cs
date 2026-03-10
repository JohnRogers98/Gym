using AutoMapper;
using Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Models.Forms;
using Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Models.Results;
using Gym.WebApplication.Features.Admin.Clients.TableView.Models.Results;
using Gym.WebApplication.Features.Admin.Instructors.Registration.Models.Forms;
using Gym.WebApplication.Features.Admin.Instructors.Registration.Models.Results;
using Gym.WebApplication.Features.Admin.Trainings.Creation.Models.Forms;
using Gym.WebApplication.Features.Admin.Trainings.Creation.Models.Results;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Requests.CalendarEvent;
using Gym.WebDto.Requests.Instructor;
using Gym.WebDto.Requests.Training;
using Gym.WebDto.Responses.Account;
using Gym.WebDto.Responses.CalendarEvent;
using Gym.WebDto.Responses.Clients;
using Gym.WebDto.Responses.Instructor;
using Gym.WebDto.Responses.Training;

namespace Gym.WebApplication.Mappings
{
    public class DtoMapping : Profile
    {
        public DtoMapping()
        {
            #region Training
            base.CreateMap<TrainingDto, TrainingViewModel>();
            base.CreateMap<TrainingInfo, TrainingViewModel>();

            base.CreateMap<CreateTrainingFormModel, CreateTrainingRequest>();
            base.CreateMap<CreateTrainingResponse, CreateTrainingResult>();

            base.CreateMap<GetTrainingResponse, TrainingViewModel>();
            #endregion

            #region Instructor
            base.CreateMap<InstructorDto, InstructorViewModel>();
            base.CreateMap<InstructorInfo, InstructorViewModel>();

            base.CreateMap<InstructorRegistrationFormModel, CreateInstructorRequest>();
            base.CreateMap<CreateInstructorResponse, CreateInstructorResult>();

            base.CreateMap<GetInstructorResponse, InstructorViewModel>();
            #endregion

            #region CalendarEvent
            base.CreateMap<ClientCalendarEventDto, ClientCalendarItemViewModel>();
            base.CreateMap<AdminCalendarEventDto, AdminCalendarItemViewModel>();

            base.CreateMap<CreateCalendarEventFormModel, CreateCalendarEventRequest>();
            
            base.CreateMap<CreateCalendarEventResponse, CreateCalendarEventResult>();

            base.CreateMap<GetAdminCalendarEventResponse, AdminCalendarItemViewModel>();
            #endregion

            #region Client
            base.CreateMap<ClientDto, ClientViewModel>();
            base.CreateMap<GetClientResponse, ClientViewModel>();
            base.CreateMap<ChargeAccountResponse, ChargeClientResult>();
            #endregion
        }
    }
}
