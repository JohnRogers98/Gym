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
    public class GetClientDetailsService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions) : IRequestHandler<GetClientDetails, ClientViewModel>
    {
        public async Task<AsyncOperation<ClientViewModel>> HandleAsync(GetClientDetails request, CancellationToken cancellationToken = default)
        {
            using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            using var getClientDetailsRequest = this.CreateGetRequest(_bffOptions.Value.GetClientDetailsEndpoint);

            HttpResponseMessage getClientDetailsResponse = await httpClient.SendAsync(getClientDetailsRequest, cancellationToken);
            if (getClientDetailsResponse.IsSuccessStatusCode)
            {
                var deserializedGetClientDetailsResponse = await getClientDetailsResponse.Content.ReadFromJsonAsync<Response<ClientDto>>();
                if (deserializedGetClientDetailsResponse is null)
                    return AsyncOperation<ClientViewModel>.EmptyResponseBody();

                ClientViewModel clientViewModel = new()
                {
                    Id = deserializedGetClientDetailsResponse.Data.Id,
                    FirstName = deserializedGetClientDetailsResponse.Data.FirstName,
                    LastName = deserializedGetClientDetailsResponse.Data.LastName,
                    AvailableTrainingsCount = deserializedGetClientDetailsResponse.Data.AvailableTrainingsCount
                };
                return AsyncOperation<ClientViewModel>.Success(clientViewModel);
            }

            if (getClientDetailsResponse.IsContentTypeProblemDetails())
            {
                return await getClientDetailsResponse.GetFailedOperationFromProblemDetailsAsync<ClientViewModel>(cancellationToken);
            }

            return AsyncOperation<ClientViewModel>.UnknownResponseType((Int32)getClientDetailsResponse.StatusCode);
        }
    }

    public class GetClientDetails;
}
