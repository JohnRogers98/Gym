using Gym.BFF.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace Gym.BFF.Services
{
    public interface IGetUserInfoService
    {
        Task<Result<UserInfoResponse>> HandleAsync(String accessToken, CancellationToken cancellationToken); 
    }

    public class GetUserInfoService(IHttpClientFactory _httpClientFactory, IOptions<UrlsOptions> _urls) : IGetUserInfoService
    {
        //TODO: propagate token error 
        public async Task<Result<UserInfoResponse>> HandleAsync(String accessToken, CancellationToken cancellationToken)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(HttpClientNames.AuthorizationServer);

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, _urls.Value.AuthorizationServer.UserInfoEndpoint);

            requestMessage.Headers.Authorization
                = new AuthenticationHeaderValue("Bearer", accessToken);

            var tokenResponse = await httpClient.SendAsync(requestMessage, cancellationToken);
            tokenResponse.EnsureSuccessStatusCode();

            var deserializedResponse = await tokenResponse.Content.ReadFromJsonAsync<UserInfoResponse>(cancellationToken)
                ?? throw new InvalidOperationException("Failed to deserialize token response");

            return Result<UserInfoResponse>.Success(deserializedResponse);
        }
    }

    public class UserInfoResponse
    {
        [JsonPropertyName("sub")]
        public required String Subject { get; set; }

        #region profile
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public String? Name { get; set; }

        [JsonPropertyName("given_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public String? GivenName { get; set; }

        [JsonPropertyName("family_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public String? FamilyName { get; set; }
        #endregion
    }
}
