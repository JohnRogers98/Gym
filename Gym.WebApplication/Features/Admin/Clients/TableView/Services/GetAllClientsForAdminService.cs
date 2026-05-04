using AutoMapper;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features.Admin.Clients.TableView.Models;
using Gym.WebApplication.Operations;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Clients;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Clients.TableView.Services
{
    public class GetAllClientsForAdminService(HttpClient _httpClient, IMapper _mapper) : IRequestHandler<GetAllClientsForAdmin, IEnumerable<ClientViewModel>>
    {
        public async Task<AsyncOperation<IEnumerable<ClientViewModel>>> HandleAsync(GetAllClientsForAdmin request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<ClientDto>>("api/admin-clients", cancellationToken: cancellationToken);
            
            var responseData = response!.Data.Select(_mapper.Map<ClientViewModel>).ToList();
            return AsyncOperation<IEnumerable<ClientViewModel>>.Success(responseData);
        }
    }
}
