using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.Options;

namespace Gym.WebApplication.Authentication
{
    public interface ITelegramInitService
    {
        Task<AsyncOperation> HandleAsync(String initData, CancellationToken cancellationToken = default);
    }

    public class TelegramInitService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions) : ITelegramInitService
    {
        public async Task<AsyncOperation> HandleAsync(String initData, CancellationToken cancellationToken = default)
        {
            using HttpClient httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            using var messageRequest = new HttpRequestMessage(HttpMethod.Post, _bffOptions.Value.TelegramInitEndpoint);
            messageRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var formData = new Dictionary<String, String>{
                { "initData", initData }
            };
            var content = new FormUrlEncodedContent(formData);
            messageRequest.Content = content;

            var response = await httpClient.SendAsync(messageRequest, cancellationToken);
            if (response.IsSuccessStatusCode)
                return AsyncOperation.Success();

            return AsyncOperation.Failure("Invalid request", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }
}
