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
    public class GetTrainingByIdService(HttpClient _httpClient, IMapper _mapper) : IRequestHandler<GetTrainingById, TrainingViewModel>
    {
        public async Task<AsyncOperation<TrainingViewModel>> HandleAsync(GetTrainingById request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync($"api/trainings/{request.TrainingId.Value}", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<Response<TrainingDto>>(cancellationToken: cancellationToken);
                return AsyncOperation<TrainingViewModel>.Success(_mapper.Map<TrainingViewModel>(responseData!.Data));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return AsyncOperation<TrainingViewModel>.Failure("Training not found", ErrorType.NotFound);
            }

            return AsyncOperation<TrainingViewModel>.Failure($"Unknown response type.", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }
}
