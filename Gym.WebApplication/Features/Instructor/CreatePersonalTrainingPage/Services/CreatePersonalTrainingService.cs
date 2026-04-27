using AutoMapper;
using Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Models.Forms;
using Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Models.Results;
using Gym.WebDto.Requests.PersonalTraining;
using Gym.WebDto.Responses.PersonalTraining;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Services
{
    public interface ICreatePersonalTrainingService
    {
        Task<CreatePersonalTrainingResult> HandleAsync(CreatePersonalTrainingFormModel createPersonalTrainingFormModel, CancellationToken cancellationToken = default);
    }

    public class CreatePersonalTrainingService(HttpClient _httpClient, IMapper _mapper) : ICreatePersonalTrainingService
    {
        public async Task<CreatePersonalTrainingResult> HandleAsync(CreatePersonalTrainingFormModel createPersonalTrainingFormModel, CancellationToken cancellationToken = default)
        {
            var createPersonalTrainingRequest = _mapper.Map<CreatePersonalTrainingRequest>(createPersonalTrainingFormModel);

            var response = await _httpClient.PostAsJsonAsync("api/personal-trainings", createPersonalTrainingRequest, cancellationToken);
            var createPersonalTrainingResponse = await response.Content.ReadFromJsonAsync<CreatePersonalTrainingResponse>();

            return _mapper.Map<CreatePersonalTrainingResult>(createPersonalTrainingResponse);
        }
    }
}
