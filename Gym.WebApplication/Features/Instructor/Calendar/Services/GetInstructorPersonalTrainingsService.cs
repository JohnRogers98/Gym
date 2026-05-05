using AutoMapper;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.PersonalTraining;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Instructor.Calendar.Services
{
    public class GetInstructorPersonalTrainingsService(HttpClient _httpClient, IMapper _mapper) 
        : IRequestHandler<GetInstructorPersonalTrainings, IEnumerable<PersonalTrainingViewModel>>
    {
        public async Task<AsyncOperation<IEnumerable<PersonalTrainingViewModel>>> HandleAsync(GetInstructorPersonalTrainings request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<PersonalTrainingDto>>("api/instructors/me/personal-trainings", cancellationToken: cancellationToken);

            var responseData = response!.Data.Select(_mapper.Map<PersonalTrainingViewModel>).ToList();
            return AsyncOperation<IEnumerable<PersonalTrainingViewModel>>.Success(responseData);
        }
    }

    public class GetInstructorPersonalTrainings;
}
