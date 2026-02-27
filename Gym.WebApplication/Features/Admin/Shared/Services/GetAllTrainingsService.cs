using AutoMapper;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Training;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Shared.Services
{
    public interface IGetAllTrainingsService
    {
        Task<IEnumerable<TrainingViewModel>> ExecuteAsync(CancellationToken cancellationToken = default);
    }

    public class GetAllTrainingsService(HttpClient _httpClient, IMapper _mapper) : IGetAllTrainingsService
    {
        public async Task<IEnumerable<TrainingViewModel>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<TrainingDto>>("api/trainings", cancellationToken: cancellationToken);
            
            IEnumerable<TrainingDto> dtos = response!.Data;
            return dtos.Select(_mapper.Map<TrainingViewModel>).ToList();
        }
    }
}
