using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;

namespace Gym.WebApplication.Features.Instructor.Calendar.Services
{
    public class CancelPersonalTrainingService(HttpClient _httpClient) : IRequestHandler<CancelPersonalTraining, CancelPersonalTrainingResult>
    {
        public async Task<AsyncOperation<CancelPersonalTrainingResult>> HandleAsync(CancelPersonalTraining request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsync(
                $"api/personal-trainings/{request.PersonalTrainingId}/actions/cancel",
                null,
                cancellationToken: cancellationToken);

            if (response.IsSuccessStatusCode)
                return AsyncOperation<CancelPersonalTrainingResult>.Success(new CancelPersonalTrainingResult());

            if (response.IsContentTypeProblemDetails())
                return await response.GetFailedOperationFromProblemDetailsAsync<CancelPersonalTrainingResult>(cancellationToken);

            return AsyncOperation<CancelPersonalTrainingResult>.Failure($"Unknown response type.", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }

    public class CancelPersonalTraining
    {
        public required String PersonalTrainingId { get; set; }
    }

    public class CancelPersonalTrainingResult;
}
