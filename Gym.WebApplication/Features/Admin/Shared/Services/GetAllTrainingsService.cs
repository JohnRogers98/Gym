using AutoMapper;
using Gym.WebApplication.Features.Admin.Trainings.States;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Training;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Shared.Services
{
    public interface IGetAllTrainingsService
    {
        Task<IEnumerable<TrainingViewModel>> HandleAsync(CancellationToken cancellationToken = default);
    }

    public class CachableGetAllTrainingsSetvice : IGetAllTrainingsService
    {
        private readonly IGetAllTrainingsService _decoratee;
        private readonly ITrainingCreationSharedState _trainingCreationSharedState;

        private IEnumerable<TrainingViewModel>? _cache;

        public CachableGetAllTrainingsSetvice(IGetAllTrainingsService decoratee, ITrainingCreationSharedState trainingCreationSharedState)
        {
            _decoratee = decoratee;
            _trainingCreationSharedState = trainingCreationSharedState;

            _trainingCreationSharedState.TrainingCreated += _ => _cache = null; 
        }

        public async Task<IEnumerable<TrainingViewModel>> HandleAsync(CancellationToken cancellationToken = default)
        {
            if(_cache is not null)
            {
                return _cache;
            }

            _cache = [.. await _decoratee.HandleAsync(cancellationToken)];
            return _cache;
        }
    }

    public class GetAllTrainingsService(HttpClient _httpClient, IMapper _mapper) : IGetAllTrainingsService
    {
        public async Task<IEnumerable<TrainingViewModel>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<TrainingDto>>("api/trainings", cancellationToken: cancellationToken);
            
            IEnumerable<TrainingDto> dtos = response!.Data;
            return dtos.Select(_mapper.Map<TrainingViewModel>).ToList();
        }
    }
}
