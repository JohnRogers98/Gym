using AutoMapper;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Instructor;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Shared.Services
{
    public interface IGetAllInstructorsService
    {
        Task<IEnumerable<InstructorViewModel>> ExecuteAsync(CancellationToken cancellationToken = default);
    }

    public class GetAllInstructorsService(HttpClient _httpClient, IMapper _mapper) : IGetAllInstructorsService
    {
        public async Task<IEnumerable<InstructorViewModel>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<InstructorDto>>("api/instructors", cancellationToken: cancellationToken);
            IEnumerable<InstructorDto> dtos = response!.Data;
            return dtos.Select(_mapper.Map<InstructorViewModel>).ToList();
        }
    }
}
