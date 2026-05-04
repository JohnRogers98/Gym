using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features.Admin.Clients.Creation.Models.Forms;
using Gym.WebApplication.Features.Admin.Clients.Creation.Models.Results;
using Gym.WebApplication.Operations;
using Gym.WebDto.Requests.Client;
using Gym.WebDto.Responses.Clients;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Clients.Creation.Services
{
    public class CreateClientService(HttpClient _httpClient) : IRequestHandler<CreateClientFormModel, CreateClientResult>
    {
        public async Task<AsyncOperation<CreateClientResult>> HandleAsync(CreateClientFormModel request, CancellationToken cancellationToken = default)
        {
            CreateClientRequest createClientRequest = new()
            {
                Login = request.Login!,
                FirstName = request.FirstName!,
                LastName = request.LastName,
            };

            var response = await _httpClient.PostAsJsonAsync("api/clients", createClientRequest, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var createClientResponse = await response.Content.ReadFromJsonAsync<CreateClientResponse>(cancellationToken: cancellationToken);
                
                var createClientResult = new CreateClientResult()
                {
                    UserId = createClientResponse!.ClientId,
                    Login = createClientResponse.Login,
                    Password = createClientResponse.Password
                };

                return AsyncOperation<CreateClientResult>.Success(createClientResult);
            }

            if(response.IsContentTypeProblemDetails())
            {
                return await response.GetFailedOperationFromProblemDetailsAsync<CreateClientResult>(cancellationToken);
            }


            return AsyncOperation<CreateClientResult>.Failure($"Unknown response type.", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }
}
