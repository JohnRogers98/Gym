using Gym.BFF.Options;
using Gym.OAuth.Extensions;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace Gym.BFF.Services
{
    public interface IGetUserInfoService
    {
        Task<Result<UserInfo>> HandleAsync(String accessToken, CancellationToken cancellationToken); 
    }

    public class GetUserInfoService(IHttpClientFactory _httpClientFactory, IOptions<UrlsOptions> _urls) : IGetUserInfoService
    {
        //TODO: propagate token error 
        public async Task<Result<UserInfo>> HandleAsync(String accessToken, CancellationToken cancellationToken)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(HttpClientNames.AuthorizationServer);

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, _urls.Value.AuthorizationServer.UserInfoEndpoint);

            requestMessage.Headers.Authorization
                = new AuthenticationHeaderValue("Bearer", accessToken);

            var tokenResponse = await httpClient.SendAsync(requestMessage, cancellationToken);
            tokenResponse.EnsureSuccessStatusCode();

            var deserializedResponse = await tokenResponse.Content.ReadFromJsonAsync<UserInfo>(cancellationToken)
                ?? throw new InvalidOperationException("Failed to deserialize token response");

            return Result<UserInfo>.Success(deserializedResponse);
        }
    }
}
