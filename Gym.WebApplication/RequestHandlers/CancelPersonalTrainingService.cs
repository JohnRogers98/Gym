using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Microsoft.Extensions.Options;

namespace Gym.WebApplication.RequestHandlers
{
    public class CancelPersonalTrainingService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions) 
        : IRequestHandler<CancelPersonalTraining, CancelPersonalTrainingResult>
    {
        public async Task<AsyncOperation<CancelPersonalTrainingResult>> HandleAsync(CancelPersonalTraining request, CancellationToken cancellationToken = default)
        {
            using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            var endpointUrl = UrlHelper.ReplacePathVariables(_bffOptions.Value.CancelPersonalTrainingEndpoint, new() { ["personalTrainingId"] = request.PersonalTrainingId! });

            using var cancelPersonalTrainingRequest = this.CreateEmptyPostRequest(endpointUrl);

            var cancelPersonalTrainingResponse = await httpClient.SendAsync(cancelPersonalTrainingRequest, cancellationToken);
            if (cancelPersonalTrainingResponse.IsSuccessStatusCode)
            {
                return AsyncOperation<CancelPersonalTrainingResult>.Success(new());
            }

            if (cancelPersonalTrainingResponse.IsContentTypeProblemDetails())
            {
                return await cancelPersonalTrainingResponse.GetFailedOperationFromProblemDetailsAsync<CancelPersonalTrainingResult>(cancellationToken);
            }

            return AsyncOperation<CancelPersonalTrainingResult>.UnknownResponseType((Int32)cancelPersonalTrainingResponse.StatusCode);
        }
    }

    public class CancelPersonalTraining
    {
        public required String PersonalTrainingId { get; set; }
    }

    public class CancelPersonalTrainingResult;
}
