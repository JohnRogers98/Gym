using AutoMapper;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features.Admin.Trainings.Creation.Models.Forms;
using Gym.WebApplication.Features.Admin.Trainings.Creation.Models.Results;
using Gym.WebApplication.Operations;
using Gym.WebDto.Requests.Training;
using Gym.WebDto.Responses.Training;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Trainings.Creation.Services
{
    public class CreateTrainingService(HttpClient _httpClient, IMapper _mapper) : IRequestHandler<CreateTrainingFormModel, CreateTrainingResult>
    {
        public async Task<AsyncOperation<CreateTrainingResult>> HandleAsync(CreateTrainingFormModel request, CancellationToken cancellationToken)
        {
            var createTrainingRequest = _mapper.Map<CreateTrainingRequest>(request);

            var response = await _httpClient.PostAsJsonAsync("api/trainings", createTrainingRequest, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var createTrainingResponse = await response.Content.ReadFromJsonAsync<CreateTrainingResponse>(cancellationToken: cancellationToken);
                return AsyncOperation<CreateTrainingResult>.Success(
                    _mapper.Map<CreateTrainingResult>(createTrainingResponse));
            }
            
            if(response.IsContentTypeProblemDetails())
                return await response.GetFailedOperationFromProblemDetailsAsync<CreateTrainingResult>(cancellationToken);

            return AsyncOperation<CreateTrainingResult>.Failure($"Unknown response type.", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }
}
