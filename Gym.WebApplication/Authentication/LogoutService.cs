using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.Options;

namespace Gym.WebApplication.Authentication
{
    public interface ILogoutService
    {
        Task<AsyncOperation> HandleAsync(CancellationToken cancellationToken = default);
    }

    public class LogoutService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions) : ILogoutService
    {
        public async Task<AsyncOperation> HandleAsync(CancellationToken cancellationToken = default)
        {
            using HttpClient httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            using var messageRequest = new HttpRequestMessage(HttpMethod.Post, _bffOptions.Value.LogoutEndpoint);
            messageRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var response = await httpClient.SendAsync(messageRequest, cancellationToken);
            if (response.IsSuccessStatusCode)
                return AsyncOperation.Success();

            return AsyncOperation.Failure("Invalid request", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }
}
