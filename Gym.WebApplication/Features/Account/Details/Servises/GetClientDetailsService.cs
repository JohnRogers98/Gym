using AutoMapper;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses.Clients;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Account.Details.Servises
{
    public interface IGetClientDetailsService
    {
        Task<ClientViewModel> HandleAsync(CancellationToken cancellationToken = default);
    }

    public class GetClientDetailsService(HttpClient _httpClient, IMapper _mapper) : IGetClientDetailsService
    {
        public async Task<ClientViewModel> HandleAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<GetClientResponse>("api/clients", cancellationToken: cancellationToken);
            return _mapper.Map<ClientViewModel>(response);
        }
    }
}
