using AutoMapper;
using Gym.Abstractions.Query._CommonInfos;
using Gym.Abstractions.Query.CalendarEvents;
using Gym.Application.Services.BookingApi.BookTrainingEvent;
using Gym.Application.Services.CalendarEventApi.CancelCalendarEvent;
using Gym.Application.Services.CalendarEventApi.CreateCalendarEvent;
using Gym.WebDto.Requests.CalendarEvent;
using Gym.WebDto.Responses.Bookings;
using Gym.WebDto.Responses.CalendarEvent;

namespace Gym.WebApi.Controllers.Api.CalendarEvents
{
    public class _DtoMappings : Profile
    {
        public _DtoMappings()
        {
            CreateMap<CreateCalendarEventRequest, CreateCalendarEvent>();
            CreateMap<CreateCalendarEventResult, CreateCalendarEventResponse>();

            CreateMap<CalendarEventPollDto, CalendarEventPoll>();
            CreateMap<CalendarEventPollResponseDto, CalendarEventPollResponse>();

            CreateMap<ChoiceInfo, ChoiceStateInfo>();
            CreateMap<PollInfo, CalendarEventPollStateDto>();

            CreateMap<CalendarEventProjection, AdminCalendarEventDto>();

            CreateMap<CalendarEventProjection, ClientCalendarEventDto>()
              .ForMember(dest => dest.IsAlreadyBooked,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    var currentUserId = (String)context.Items["CurrentUserId"];
                    return src.BookingUsers?.Any(aUserInfo => aUserInfo.Id == currentUserId);
                }))
              .ForMember(dest => dest.CurrentClientCount,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.BookingUsers?.Count();
                }));

            CreateMap<BookTrainingEventResult, BookTrainingEventResponse>();

            CreateMap<Abstractions.Query.CalendarEvents.TrainingInfo, WebDto.Responses.Training.TrainingInfo>();
            CreateMap<InstructorInfo, WebDto.Responses.Instructor.InstructorInfo>();
            CreateMap<Abstractions.Query.CalendarEvents.BookingUserInfo, WebDto.Responses.Bookings.BookingUserInfo>();

            CreateMap<CancelCalendarEventResult, CancelCalendarEventResponse>();
        }
    }
}
