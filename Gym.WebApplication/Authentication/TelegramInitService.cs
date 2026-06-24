using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
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

            var formData = new Dictionary<String, String>{
                { "initData", initData }
            };
            var content = new FormUrlEncodedContent(formData);

            var response = await httpClient.PostAsync(_bffOptions.Value.FullTelegramInitPath, content, cancellationToken);

            if (response.IsSuccessStatusCode)
                return AsyncOperation.Success();

            return AsyncOperation.Failure("Invalid request", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }
}
