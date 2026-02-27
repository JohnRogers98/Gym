using AutoMapper;
using Gym.WebApplication.Features.Admin.Shared.ValueObjects;
using Gym.WebApplication.Providers;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses.Instructor;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Shared.Services
{
    public interface IGetInstructorByIdService
    {
        Task<InstructorViewModel?> ExecuteAsync(InstructorId instructorId, CancellationToken cancellationToken = default);
    }

    public class RetryableGetInstructorByIdService(
        IGetInstructorByIdService _decoratee,
        IPipelineProvider _pipelineProvider) : IGetInstructorByIdService
    {
        public async Task<InstructorViewModel?> ExecuteAsync(InstructorId instructorId, CancellationToken cancellationToken = default)
        {
            return await _pipelineProvider.InstructorEventualConsistency.ExecuteAsync(async token =>
            {
                return await _decoratee.ExecuteAsync(instructorId, token);
            }, cancellationToken);
        }
    }

    public class GetInstructorByIdService(HttpClient _httpClient, IMapper _mapper) : IGetInstructorByIdService
    {
        public async Task<InstructorViewModel?> ExecuteAsync(InstructorId instructorId, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<GetInstructorResponse>($"api/instructors/{instructorId.Value}", cancellationToken: cancellationToken);

            if(response is not null)
                return _mapper.Map<InstructorViewModel>(response);
            return null;
        }
    }
}
