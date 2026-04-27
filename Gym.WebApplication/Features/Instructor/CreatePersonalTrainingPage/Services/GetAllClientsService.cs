using AutoMapper;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Clients;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Services
{
    public interface IGetAllClientsService
    {
        Task<IEnumerable<ClientViewModel>> HandleAsync(CancellationToken cancellationToken = default);
    }

    public class GetAllClientsService(HttpClient _httpClient, IMapper _mapper) : IGetAllClientsService
    {
        public async Task<IEnumerable<ClientViewModel>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<ClientDto>>("api/instructor-clients", cancellationToken: cancellationToken);
            IEnumerable<ClientDto> dtos = response!.Data;
            return dtos.Select(_mapper.Map<ClientViewModel>).ToList();
        }
    }
}
