using AutoMapper;
using Gym.WebApplication.Features.Admin.Instructors.Registration.Models.Forms;
using Gym.WebApplication.Features.Admin.Instructors.Registration.Models.Results;
using Gym.WebDto.Requests.Instructor;
using Gym.WebDto.Responses.Instructor;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Instructors.Registration.Services
{
    public interface ICreateInstructorService
    {
        Task<CreateInstructorResult> ExecuteAsync(InstructorRegistrationFormModel registrationFormModel, CancellationToken cancellationToken = default);
    }

    public class CreateInstructorService(HttpClient _httpClient, IMapper _mapper) : ICreateInstructorService
    {
        public async Task<CreateInstructorResult> ExecuteAsync(InstructorRegistrationFormModel registrationFormModel, CancellationToken cancellationToken = default)
        {
            var createInstructorRequest = _mapper.Map<CreateInstructorRequest>(registrationFormModel);
            var response = await _httpClient.PostAsJsonAsync("api/instructors", createInstructorRequest, cancellationToken);
            var createInstructorResponse = await response.Content.ReadFromJsonAsync<CreateInstructorResponse>();
            return _mapper.Map<CreateInstructorResult>(createInstructorResponse);
        }
    }
}
