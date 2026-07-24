using AutoMapper;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Clients;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Gym.WebApplication.RequestHandlers
{
    public class ListClientsForInstructorService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions, IMapper _mapper) 
        : IRequestHandler<ListClientsForInstructor, IEnumerable<ClientViewModel>>
    {
        public async Task<AsyncOperation<IEnumerable<ClientViewModel>>> HandleAsync(ListClientsForInstructor request, CancellationToken cancellationToken = default)
        {
            using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            using var listClientsRequest = this.CreateGetRequest(_bffOptions.Value.ListClientsForInstructorEndpoint);

            HttpResponseMessage listClientsResponse = await httpClient.SendAsync(listClientsRequest, cancellationToken);
            if (listClientsResponse.IsSuccessStatusCode)
            {
                var deserializedResponse = await listClientsResponse.Content.ReadFromJsonAsync<ListResponse<ClientDto>>();
                if (deserializedResponse is null)
                    return AsyncOperation<IEnumerable<ClientViewModel>>.EmptyResponseBody();

                var instructors = deserializedResponse.Data.Select(_mapper.Map<ClientViewModel>);
                return AsyncOperation<IEnumerable<ClientViewModel>>.Success(instructors);
            }

            if (listClientsResponse.IsContentTypeProblemDetails())
            {
                return await listClientsResponse.GetFailedOperationFromProblemDetailsAsync<IEnumerable<ClientViewModel>>(cancellationToken);
            }

            return AsyncOperation<IEnumerable<ClientViewModel>>.UnknownResponseType((Int32)listClientsResponse.StatusCode);
        }
    }

    public class ListClientsForInstructor;
}
