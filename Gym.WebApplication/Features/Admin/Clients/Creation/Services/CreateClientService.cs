using Gym.WebApplication.Features.Admin.Clients.Creation.Models.Forms;
using Gym.WebApplication.Features.Admin.Clients.Creation.Models.Results;
using Gym.WebDto.Requests.Client;
using Gym.WebDto.Responses.Clients;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Clients.Creation.Services
{
    public interface ICreateClientService
    {
        Task<CreateClientResult> HandleAsync(CreateClientFormModel createClientFormModel, CancellationToken cancellationToken = default);
    }

    public class CreateClientService(HttpClient _httpClient) : ICreateClientService
    {
        public async Task<CreateClientResult> HandleAsync(CreateClientFormModel createClientFormModel, CancellationToken cancellationToken = default)
        {
            CreateClientRequest createUserRequest = new()
            {
                Login = createClientFormModel.Login!,
                FirstName = createClientFormModel.FirstName!,
                LastName = createClientFormModel.LastName,
            };

            var response = await _httpClient.PostAsJsonAsync("api/clients", createUserRequest, cancellationToken);
            var createUserResponse = await response.Content.ReadFromJsonAsync<CreateClientResponse>();

            return new CreateClientResult()
            {
                UserId = createUserResponse!.ClientId,
                Login = createUserResponse.Login,
                Password = createUserResponse.Password
            };
        }
    }
}
