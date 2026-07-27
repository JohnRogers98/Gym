using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Gym.WebApplication.Authentication
{
    public interface ICheckSessionService
    {
        Task<AsyncOperation> HandleAsync(CancellationToken cancellationToken = default);
    }

    public class CheckSessionService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions) : ICheckSessionService
    {
        public async Task<AsyncOperation> HandleAsync(CancellationToken cancellationToken = default)
        {
            using HttpClient httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            using var messageRequest = new HttpRequestMessage(HttpMethod.Get, _bffOptions.Value.CheckSessionEndpoint);
            messageRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include); 

            var response = await httpClient.SendAsync(messageRequest, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var checkSession = await response.Content.ReadFromJsonAsync<CheckSessionResponse>(cancellationToken);
                if (checkSession is not null && checkSession.Authenticated)
                    return AsyncOperation.Success();

                return AsyncOperation.Failure("Session does not set", ErrorType.NotFound);
            }

            return AsyncOperation.Failure("Unknown status-code", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }

    public record CheckSessionResponse(Boolean Authenticated);
}
