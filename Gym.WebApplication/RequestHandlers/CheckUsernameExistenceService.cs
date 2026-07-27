using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebDto.Requests.Users;
using Gym.WebDto.Responses.Users;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Gym.WebApplication.RequestHandlers
{
    public class CheckUsernameExistenceService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions) 
        : IRequestHandler<CheckUsernameExistence, CheckUsernameExistenceResult>
    {
        public async Task<AsyncOperation<CheckUsernameExistenceResult>> HandleAsync(CheckUsernameExistence request, CancellationToken cancellationToken = default)
        {
            using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            using var checkUsernameRequest = this.CreatePostRequestWithJson(
                _bffOptions.Value.CheckUsernameEndpoint,
                new CheckUsernameExistenceRequest { Username = request.Username }
            );

            var checkUsernameExistenceResponse = await httpClient.SendAsync(checkUsernameRequest, cancellationToken);
            if (checkUsernameExistenceResponse.IsSuccessStatusCode)
            {
                var deserializedCheckUsernameExistenceResponse = await checkUsernameExistenceResponse.Content.ReadFromJsonAsync<CheckUsernameExistenceResponse>();
                if (deserializedCheckUsernameExistenceResponse is null)
                    return AsyncOperation<CheckUsernameExistenceResult>.EmptyResponseBody();

                return AsyncOperation<CheckUsernameExistenceResult>.Success(new CheckUsernameExistenceResult(deserializedCheckUsernameExistenceResponse.IsExist));
            }

            if (checkUsernameExistenceResponse.IsContentTypeProblemDetails())
            {
                return await checkUsernameExistenceResponse.GetFailedOperationFromProblemDetailsAsync<CheckUsernameExistenceResult>(cancellationToken);
            }

            return AsyncOperation<CheckUsernameExistenceResult>.UnknownResponseType((Int32)checkUsernameExistenceResponse.StatusCode);
        }
    }

    public record CheckUsernameExistence(String Username);

    public record CheckUsernameExistenceResult(Boolean IsExist);
}
