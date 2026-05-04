using AutoMapper;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features.Admin.Shared.Models;
using Gym.WebApplication.Operations;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Training;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Shared.Services
{
    public class GetAllTrainingsService(HttpClient _httpClient, IMapper _mapper) : IRequestHandler<GetAllTrainings, IEnumerable<TrainingViewModel>>
    {
        public async Task<AsyncOperation<IEnumerable<TrainingViewModel>>> HandleAsync(GetAllTrainings request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<TrainingDto>>("api/trainings", cancellationToken: cancellationToken);
            
            var items = response!.Data.Select(_mapper.Map<TrainingViewModel>).ToList();
            return AsyncOperation<IEnumerable<TrainingViewModel>>.Success(items);
        }
    }
}
