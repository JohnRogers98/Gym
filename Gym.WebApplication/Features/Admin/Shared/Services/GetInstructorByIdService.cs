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
    public class GetInstructorByIdService(HttpClient _httpClient, IMapper _mapper) : IRequestHandler<GetInstructorById, InstructorViewModel>
    {
        public async Task<AsyncOperation<InstructorViewModel>> HandleAsync(GetInstructorById request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync($"api/instructors/{request.InstructorId.Value}", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<Response<InstructorDto>>(cancellationToken: cancellationToken);
                return AsyncOperation<InstructorViewModel>.Success(_mapper.Map<InstructorViewModel>(responseData!.Data));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return AsyncOperation<InstructorViewModel>.Failure("Instructor not found", ErrorType.NotFound);
            }

            return AsyncOperation<InstructorViewModel>.Failure($"Unknown response type.", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }
}
