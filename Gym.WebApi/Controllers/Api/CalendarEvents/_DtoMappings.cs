using AutoMapper;
using Gym.Application.Services.BookingApi;
using Gym.Application.Services.CalendarEventApi;
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
            CreateMap<CalendarEventDetails, CreateCalendarEventResponse>();

            CreateMap<CalendarEventDetails, GetAdminCalendarEventResponse>();
            CreateMap<CalendarEventDetails, AdminCalendarEventDto>();

            CreateMap<CalendarEventDetails, GetClientCalendarEventResponse>()
              .ForMember(dest => dest.IsAlreadyBooked,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    var currentUserId = (String)context.Items["CurrentUserId"];
                    return src.BookingUsers.Any(aUserId => aUserId == currentUserId);
                }))
              .ForMember(dest => dest.CurrentClientCount,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.BookingUsers?.Count();
                }));

            CreateMap<CalendarEventDetails, ClientCalendarEventDto>()
              .ForMember(dest => dest.IsAlreadyBooked,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    var currentUserId = (String)context.Items["CurrentUserId"];
                    return src.BookingUsers.Any(aUserId => aUserId == currentUserId);
                }))
              .ForMember(dest => dest.CurrentClientCount,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    return src.BookingUsers?.Count();
                }));

            CreateMap<BookingDetails, BookTrainingEventResponse>();
        }
    }
}
