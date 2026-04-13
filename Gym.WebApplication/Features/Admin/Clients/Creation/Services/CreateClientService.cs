using Gym.WebApplication.Features.Admin.Clients.Creation.Models.Forms;
using Gym.WebApplication.Features.Admin.Clients.Creation.Models.Results;
using Gym.WebDto.Requests.Users;
using Gym.WebDto.Responses.Users;
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
            CreateUserRequest createUserRequest = new()
            {
                Login = createClientFormModel.Login!,
                Role = "Client",
                FirstName = createClientFormModel.FirstName!,
                LastName = createClientFormModel.LastName,
            };

            var response = await _httpClient.PostAsJsonAsync("api/users", createUserRequest, cancellationToken);
            var createUserResponse = await response.Content.ReadFromJsonAsync<CreateUserResponse>();

            return new CreateClientResult()
            {
                UserId = createUserResponse!.UserId,
                Login = createUserResponse.Login,
                Password = createUserResponse.Password
            };
        }
    }
}
