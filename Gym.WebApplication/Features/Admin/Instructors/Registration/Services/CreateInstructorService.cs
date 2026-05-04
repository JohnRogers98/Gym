using AutoMapper;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features.Admin.Instructors.Registration.Models.Forms;
using Gym.WebApplication.Features.Admin.Instructors.Registration.Models.Results;
using Gym.WebApplication.Operations;
using Gym.WebDto.Requests.Instructor;
using Gym.WebDto.Responses.Instructor;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Instructors.Registration.Services
{
    public class CreateInstructorService(HttpClient _httpClient, IMapper _mapper) : IRequestHandler<InstructorRegistrationFormModel, CreateInstructorResult>
    {
        public async Task<AsyncOperation<CreateInstructorResult>> HandleAsync(InstructorRegistrationFormModel registrationFormModel, CancellationToken cancellationToken = default)
        {
            var createInstructorRequest = _mapper.Map<CreateInstructorRequest>(registrationFormModel);

            var response = await _httpClient.PostAsJsonAsync("api/instructors", createInstructorRequest, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var createInstructorResponse = await response.Content.ReadFromJsonAsync<CreateInstructorResponse>(cancellationToken: cancellationToken);
                return AsyncOperation<CreateInstructorResult>.Success(
                    _mapper.Map<CreateInstructorResult>(createInstructorResponse));
            }

            if (response.IsContentTypeProblemDetails())
                return await response.GetFailedOperationFromProblemDetailsAsync<CreateInstructorResult>(cancellationToken);

            return AsyncOperation<CreateInstructorResult>.Failure($"Unknown response type.", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }
}
