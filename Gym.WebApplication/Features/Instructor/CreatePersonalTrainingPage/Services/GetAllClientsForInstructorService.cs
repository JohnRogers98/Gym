using AutoMapper;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Models;
using Gym.WebApplication.Operations;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Clients;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Services
{
    public class GetAllClientsForInstructorService(HttpClient _httpClient, IMapper _mapper) : IRequestHandler<GetAllClientsForInstructor, IEnumerable<ClientViewModel>>
    {
        public async Task<AsyncOperation<IEnumerable<ClientViewModel>>> HandleAsync(GetAllClientsForInstructor request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<ClientDto>>("api/instructor-clients", cancellationToken: cancellationToken);

            var responseData = response!.Data.Select(_mapper.Map<ClientViewModel>).ToList();
            return AsyncOperation<IEnumerable<ClientViewModel>>.Success(responseData);
        }
    }
}
