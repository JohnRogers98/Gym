using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Gym.WebApplication.Authentication
{
    public interface IUserInfoService
    {
        Task<AsyncOperation<UserInfoResponse>> HandleAsync(CancellationToken cancellationToken = default);
    }

    public class UserInfoService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions) : IUserInfoService
    {
        public async Task<AsyncOperation<UserInfoResponse>> HandleAsync(CancellationToken cancellationToken = default)
        {
            using HttpClient httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            var messageRequest = new HttpRequestMessage(HttpMethod.Get, _bffOptions.Value.UserInfoEndpoint);
            messageRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var response = await httpClient.SendAsync(messageRequest, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var userInfo = await response.Content.ReadFromJsonAsync<UserInfoResponse>();
                if (userInfo is not null)
                    return AsyncOperation<UserInfoResponse>.Success(userInfo);
            }

            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return AsyncOperation<UserInfoResponse>.Failure("Unauthorized", ErrorType.Forbidden);

            return AsyncOperation<UserInfoResponse>.Failure("Unknown status-code", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }

    public record UserInfoResponse
    {
        [JsonPropertyName("sub")]
        public required String UserId { get; init; }

        [JsonPropertyName("role")]
        public required String Role { get; init; }
        public String? Name { get; init; }
    }
}
