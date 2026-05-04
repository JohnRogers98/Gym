using AutoMapper;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features.Admin.Shared.Models;
using Gym.WebApplication.Operations;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Instructor;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Shared.Services
{
    public class GetAllInstructorsService(HttpClient _httpClient, IMapper _mapper) : IRequestHandler<GetAllInstructors, IEnumerable<InstructorViewModel>>
    {
        public async Task<AsyncOperation<IEnumerable<InstructorViewModel>>> HandleAsync(GetAllInstructors request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<InstructorDto>>("api/instructors", cancellationToken: cancellationToken);

            var responseData = response!.Data.Select(_mapper.Map<InstructorViewModel>).ToList();
            return AsyncOperation<IEnumerable<InstructorViewModel>>.Success(responseData);
        }
    }
}
