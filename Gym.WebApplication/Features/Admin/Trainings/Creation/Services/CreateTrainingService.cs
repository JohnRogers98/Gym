using AutoMapper;
using Gym.WebApplication.Features.Admin.Trainings.Creation.Models.Forms;
using Gym.WebApplication.Features.Admin.Trainings.Creation.Models.Results;
using Gym.WebDto.Requests.Training;
using Gym.WebDto.Responses.Training;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Trainings.Creation.Services
{
    public interface ICreateTrainingService
    {
        Task<CreateTrainingResult> ExecuteAsync(CreateTrainingFormModel createTrainingFormModel, CancellationToken cancellationToken = default);
    }

    public class CreateTrainingService(HttpClient _httpClient, IMapper _mapper) : ICreateTrainingService
    {
        public async Task<CreateTrainingResult> ExecuteAsync(CreateTrainingFormModel createTrainingFormModel, CancellationToken cancellationToken = default)
        {
            var createTrainingRequest = _mapper.Map<CreateTrainingRequest>(createTrainingFormModel);

            var response = await _httpClient.PostAsJsonAsync("api/trainings", createTrainingRequest, cancellationToken);
            var createTrainingResponse = await response.Content.ReadFromJsonAsync<CreateTrainingResponse>();

            return _mapper.Map<CreateTrainingResult>(createTrainingResponse);
        }
    }
}
