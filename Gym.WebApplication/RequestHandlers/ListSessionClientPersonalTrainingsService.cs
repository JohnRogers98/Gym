using AutoMapper;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.PersonalTraining;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Gym.WebApplication.RequestHandlers
{
    public class ListSessionClientPersonalTrainingsService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions, IMapper _mapper) 
        : IRequestHandler<ListSessionClientPersonalTrainings, IEnumerable<PersonalTrainingViewModel>>
    {
        public async Task<AsyncOperation<IEnumerable<PersonalTrainingViewModel>>> HandleAsync(ListSessionClientPersonalTrainings request, CancellationToken cancellationToken = default)
        {
            using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            using var listPersonalTrainingsRequest = this.CreateGetRequest(_bffOptions.Value.ListSessionClientPersonalTrainingsEndpoint);

            HttpResponseMessage listPersonalTrainingsResponse = await httpClient.SendAsync(listPersonalTrainingsRequest, cancellationToken);
            if (listPersonalTrainingsResponse.IsSuccessStatusCode)
            {
                var deserializedResponse = await listPersonalTrainingsResponse.Content.ReadFromJsonAsync<ListResponse<PersonalTrainingDto>>();
                if (deserializedResponse is null)
                    return AsyncOperation<IEnumerable<PersonalTrainingViewModel>>.EmptyResponseBody();

                var personalTrainings = deserializedResponse.Data.Select(_mapper.Map<PersonalTrainingViewModel>);
                return AsyncOperation<IEnumerable<PersonalTrainingViewModel>>.Success(personalTrainings);
            }

            if (listPersonalTrainingsResponse.IsContentTypeProblemDetails())
            {
                return await listPersonalTrainingsResponse.GetFailedOperationFromProblemDetailsAsync<IEnumerable<PersonalTrainingViewModel>>(cancellationToken);
            }

            return AsyncOperation<IEnumerable<PersonalTrainingViewModel>>.UnknownResponseType((Int32)listPersonalTrainingsResponse.StatusCode);
        }
    }

    public class ListSessionClientPersonalTrainings;
}
