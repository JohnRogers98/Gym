using AutoMapper;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Clients;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Clients.TableView.Services
{
    public interface IGetAllClientsService
    {
        Task<IEnumerable<ClientViewModel>> ExecuteAsync(CancellationToken cancellationToken = default);
    }

    public class GetAllClientsService(HttpClient _httpClient, IMapper _mapper) : IGetAllClientsService
    {
        public async Task<IEnumerable<ClientViewModel>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<ClientDto>>("api/admin-clients", cancellationToken: cancellationToken);
            IEnumerable<ClientDto> dtos = response!.Data;
            return dtos.Select(_mapper.Map<ClientViewModel>).ToList();
        }
    }
}
