using AutoMapper;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Training;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Gym.WebApplication.RequestHandlers
{
    public class ListTrainingsService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions, IMapper _mapper) 
        : IRequestHandler<ListTrainings, IEnumerable<TrainingViewModel>>
    {
        public async Task<AsyncOperation<IEnumerable<TrainingViewModel>>> HandleAsync(ListTrainings request, CancellationToken cancellationToken = default)
        {
            using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            using var listTrainingsRequest = this.CreateGetRequest(_bffOptions.Value.ListTrainingsEndpoint);

            HttpResponseMessage listTrainingsResponse = await httpClient.SendAsync(listTrainingsRequest, cancellationToken);
            if (listTrainingsResponse.IsSuccessStatusCode)
            {
                var deserializedResponse = await listTrainingsResponse.Content.ReadFromJsonAsync<ListResponse<TrainingDto>>();
                if (deserializedResponse is null)
                    return AsyncOperation<IEnumerable<TrainingViewModel>>.EmptyResponseBody();

                var trainings = deserializedResponse.Data.Select(_mapper.Map<TrainingViewModel>);
                return AsyncOperation<IEnumerable<TrainingViewModel>>.Success(trainings);
            }

            if (listTrainingsResponse.IsContentTypeProblemDetails())
            {
                return await listTrainingsResponse.GetFailedOperationFromProblemDetailsAsync<IEnumerable<TrainingViewModel>>(cancellationToken);
            }

            return AsyncOperation<IEnumerable<TrainingViewModel>>.UnknownResponseType((Int32)listTrainingsResponse.StatusCode);
        }
    }

    public class ListTrainings;
}
