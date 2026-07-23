using AutoMapper;
using Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Models.Forms;
using Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Models.Results;
using Gym.WebApplication.RequestHandlers;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Requests.CalendarEvent;
using Gym.WebDto.Requests.PersonalTraining;
using Gym.WebDto.Responses.Account;
using Gym.WebDto.Responses.Bookings;
using Gym.WebDto.Responses.CalendarEvent;
using Gym.WebDto.Responses.Clients;
using Gym.WebDto.Responses.Instructor;
using Gym.WebDto.Responses.PersonalTraining;
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
            #endregion

            #region Instructor
            base.CreateMap<InstructorDto, InstructorViewModel>();
            base.CreateMap<WebDto.Responses.Instructor.InstructorInfo, InstructorViewModel>();
            #endregion

            #region CalendarEvent
            base.CreateMap<ClientCalendarEventDto, CalendarEventForClientViewModel>()
                .ForMember(dest => dest.UtcStart,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.Start;
                }))
                .ForMember(dest => dest.UtcEnd,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.End;
                }))
                .ForMember(dest => dest.Poll,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.PollInfo;
                }));

            base.CreateMap<AdminCalendarEventDto, CalendarEventForAdminViewModel>()
                .ForMember(dest => dest.UtcStart,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.Start;
                }))
                .ForMember(dest => dest.UtcEnd,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.End;
                }));

            base.CreateMap<CreateCalendarEventFormModel, CreateCalendarEventRequest>()
                .ForMember(dest => dest.Start,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.UtcStart;
                }))
                .ForMember(dest => dest.End,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.UtcEnd;
                }))
                .ForMember(dest => dest.Poll,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.PollFormModel;
                }));

            base.CreateMap<CreateCalendarEventResponse, CreateCalendarEventResult>();

            base.CreateMap<BookingUserInfo, BookingUserViewModel>();
            #endregion

            #region Client
            base.CreateMap<ClientDto, ClientViewModel>();
            base.CreateMap<ChargeAccountResponse, ChargeClientResult>();
            base.CreateMap<ClientInfo, ClientViewModel>();
            #endregion

            #region Poll
            base.CreateMap<CalendarEventPollStateDto, PollViewModel>()
                 .ForMember(dest => dest.IsRequired,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.IsResponseRequired;
                }))
                .ForMember(dest => dest.CanSelectMany,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.CanAcceptMany;
                }));


            base.CreateMap<ChoiceStateInfo, ChoiceViewModel>();

            base.CreateMap <CreatePollFormModel, CalendarEventPollDto>()
                .ForMember(dest => dest.IsResponseRequired,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.IsRequired;
                }))
                .ForMember(dest => dest.CanAcceptMany,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.CanSelectMany;
                }))
                .ForMember(dest => dest.ChoiceVariants,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.Choices;
                }));

            base.CreateMap<PollResponse, CalendarEventPollResponseDto>();
            #endregion

            #region PersonalTraining
            base.CreateMap<CreatePersonalTrainingFormModel, CreatePersonalTrainingRequest>()
                .ForMember(dest => dest.Start,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.UtcStart;
                }))
                .ForMember(dest => dest.End,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.UtcEnd;
                }));

            base.CreateMap<CreatePersonalTrainingResponse, CreatePersonalTrainingResult>();

            base.CreateMap<PersonalTrainingDto, PersonalTrainingViewModel>() 
                .ForMember(dest => dest.UtcStart,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.Start;
                }))
                .ForMember(dest => dest.UtcEnd,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.End;
                }));
            #endregion
        }
    }
}
