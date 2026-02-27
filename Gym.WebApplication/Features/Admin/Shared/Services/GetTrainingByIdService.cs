using AutoMapper;
using Gym.WebApplication.Features.Admin.Shared.ValueObjects;
using Gym.WebApplication.Providers;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses.Training;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Shared.Services
{
    public interface IGetTrainingByIdService
    {
        Task<TrainingViewModel?> ExecuteAsync(TrainingId trainingId, CancellationToken cancellationToken = default);
    }

    public class RetryableGetTrainingByIdService(IGetTrainingByIdService _decoratee, IPipelineProvider _pipelineProvider) : IGetTrainingByIdService
    {
        public async Task<TrainingViewModel?> ExecuteAsync(TrainingId trainingId, CancellationToken cancellationToken = default)
        {
            return await _pipelineProvider.TrainingEventualConsistency.ExecuteAsync(async innerToken =>
            {
                return await _decoratee.ExecuteAsync(trainingId, innerToken);
            }, cancellationToken);
        }
    }

    public class GetTrainingByIdService(HttpClient _httpClient, IMapper _mapper) : IGetTrainingByIdService
    {
        public async Task<TrainingViewModel?> ExecuteAsync(TrainingId trainingId, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<GetTrainingResponse>($"api/trainings/{trainingId.Value}", cancellationToken: cancellationToken);

            if (response is not null)
                return _mapper.Map<TrainingViewModel>(response);
            return null;
        }
    }
}
