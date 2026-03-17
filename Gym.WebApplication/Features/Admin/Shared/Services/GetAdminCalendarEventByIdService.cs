using AutoMapper;
using Gym.WebApplication.Features.Admin.Shared.ValueObjects;
using Gym.WebApplication.Providers;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.CalendarEvent;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Shared.Services
{
    public interface IGetAdminCalendarEventByIdService
    {
        Task<AdminCalendarItemViewModel?> HandleAsync(CalendarEventId calendarEventId, CancellationToken cancellationToken = default);
    }

    public class RetryableGetAdminCalendarEventByIdService(IGetAdminCalendarEventByIdService _decoratee, IPipelineProvider _pipelineProvider) 
        : IGetAdminCalendarEventByIdService
    {
        public async Task<AdminCalendarItemViewModel?> HandleAsync(CalendarEventId calendarEventId, CancellationToken cancellationToken = default)
        {
            return await _pipelineProvider.CalendarEventEventualConsistency.ExecuteAsync(async innerToken =>
            {
                return await _decoratee.HandleAsync(calendarEventId, innerToken);
            }, cancellationToken);
        }
    }

    public class GetAdminCalendarEventByIdService(HttpClient _httpClient, IMapper _mapper) : IGetAdminCalendarEventByIdService
    {
        public async Task<AdminCalendarItemViewModel?> HandleAsync(CalendarEventId calendarEventId, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<Response<AdminCalendarEventDto>>($"api/admin-calendar-events/{calendarEventId.Value}", cancellationToken: cancellationToken);

            if (response is not null)
                return _mapper.Map<AdminCalendarItemViewModel>(response.Data);
            return null;
        }
    }
}
