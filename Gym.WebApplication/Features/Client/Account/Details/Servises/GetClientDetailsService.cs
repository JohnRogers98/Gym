using AutoMapper;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features.Client.Account.Details.Models;
using Gym.WebApplication.Operations;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Clients;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Account.Details.Servises
{
    public class GetClientDetailsService(HttpClient _httpClient, IMapper _mapper) : IRequestHandler<GetClientDetails, ClientViewModel>
    {
        public async Task<AsyncOperation<ClientViewModel>> HandleAsync(GetClientDetails request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<Response<ClientDto>>("api/clients", cancellationToken: cancellationToken);
            
            return AsyncOperation<ClientViewModel>.Success(
                _mapper.Map<ClientViewModel>(response!.Data));
        }
    }
}
