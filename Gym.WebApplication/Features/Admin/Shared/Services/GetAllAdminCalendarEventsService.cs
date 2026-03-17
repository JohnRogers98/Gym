using AutoMapper;
using Gym.WebApplication.Features.Admin.CalendarEvents.States;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.CalendarEvent;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Shared.Services
{
    public interface IGetAllAdminCalendarEventsService
    {
        Task<IEnumerable<AdminCalendarItemViewModel>> HandleAsync(CancellationToken cancellationToken = default);
    }

    public class CachableGetAllAdminCalendarEventsSetvice : IGetAllAdminCalendarEventsService
    {
        private readonly IGetAllAdminCalendarEventsService _decoratee;
        private readonly ICalendarEventCreationState _calendarEventCreationState;

        private IEnumerable<AdminCalendarItemViewModel>? _cache;

        public CachableGetAllAdminCalendarEventsSetvice(
            IGetAllAdminCalendarEventsService decoratee,
            ICalendarEventCreationState calendarEventCreationState,
            ICalendarEventCancellationState calendarEventCancellationState)
        {
            _decoratee = decoratee;
            _calendarEventCreationState = calendarEventCreationState;

            _calendarEventCreationState.CalendarEventCreated += _ => _cache = null;
            calendarEventCancellationState.CalendarEventCancelled += _ => _cache = null;
        }

        public async Task<IEnumerable<AdminCalendarItemViewModel>> HandleAsync(CancellationToken cancellationToken = default)
        {
            if (_cache is not null)
            {
                return _cache;
            }

            _cache = [.. await _decoratee.HandleAsync(cancellationToken)];
            return _cache;
        }
    }

    public class GetAllAdminCalendarEventsService(HttpClient _httpClient, IMapper _mapper) : IGetAllAdminCalendarEventsService
    {
        public async Task<IEnumerable<AdminCalendarItemViewModel>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<AdminCalendarEventDto>>("api/admin-calendar-events", cancellationToken: cancellationToken);
            IEnumerable<AdminCalendarEventDto> dtos = response!.Data;
            return dtos.Select(_mapper.Map<AdminCalendarItemViewModel>).ToList();
        }
    }
}
