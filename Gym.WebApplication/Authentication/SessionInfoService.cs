using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Gym.WebApplication.Authentication
{
    public interface ISessionInfoService
    {
        Task<AsyncOperation<SessionInfoResponse>> HandleAsync(CancellationToken cancellationToken = default);
    }

    public class SessionInfoService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions) : ISessionInfoService
    {
        public async Task<AsyncOperation<SessionInfoResponse>> HandleAsync(CancellationToken cancellationToken = default)
        {
            using HttpClient httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            using var messageRequest = new HttpRequestMessage(HttpMethod.Get, _bffOptions.Value.SessionInfoEndpoint);
            messageRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var response = await httpClient.SendAsync(messageRequest, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var sessionInfo = await response.Content.ReadFromJsonAsync<SessionInfoResponse>();
                if (sessionInfo is not null)
                    return AsyncOperation<SessionInfoResponse>.Success(sessionInfo);
            }

            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return AsyncOperation<SessionInfoResponse>.Failure("Unauthorized", ErrorType.Forbidden);

            return AsyncOperation<SessionInfoResponse>.Failure("Unknown status-code", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }

    public record SessionInfoResponse
    {
        public required String UserId { get; init; }

        public required String Role { get; init; }
        public String? Name { get; init; }

        public String? AuthenticationMethod { get; init; }
    }
}
