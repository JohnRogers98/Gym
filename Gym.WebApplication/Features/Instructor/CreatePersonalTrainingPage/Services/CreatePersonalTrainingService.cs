using AutoMapper;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Models.Forms;
using Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Models.Results;
using Gym.WebApplication.Operations;
using Gym.WebDto.Requests.PersonalTraining;
using Gym.WebDto.Responses.PersonalTraining;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Services
{
    public class CreatePersonalTrainingService(HttpClient _httpClient, IMapper _mapper) 
        : IRequestHandler<CreatePersonalTrainingFormModel, CreatePersonalTrainingResult>
    {
        public async Task<AsyncOperation<CreatePersonalTrainingResult>> HandleAsync(CreatePersonalTrainingFormModel createPersonalTrainingFormModel, CancellationToken cancellationToken = default)
        {
            var createPersonalTrainingRequest = _mapper.Map<CreatePersonalTrainingRequest>(createPersonalTrainingFormModel);

            var response = await _httpClient.PostAsJsonAsync("api/personal-trainings", createPersonalTrainingRequest, cancellationToken);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            if (response.IsSuccessStatusCode)
            {
                var createPersonalTrainingResponse = await response.Content.ReadFromJsonAsync<CreatePersonalTrainingResponse>(cancellationToken: cancellationToken);

                return AsyncOperation<CreatePersonalTrainingResult>.Success(
                    _mapper.Map<CreatePersonalTrainingResult>(createPersonalTrainingResponse));
            }
            else if (response.IsContentTypeProblemDetails())
            {
                return await response.GetFailedOperationFromProblemDetailsAsync<CreatePersonalTrainingResult>(cancellationToken);
            }

            return AsyncOperation<CreatePersonalTrainingResult>.Failure($"Unknown response type.", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }
}
