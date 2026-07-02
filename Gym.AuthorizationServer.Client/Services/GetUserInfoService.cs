using Gym.AuthorizationServer.Client.Options;
using Gym.OAuth.Extensions;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Gym.AuthorizationServer.Client.Services
{
    public interface IGetUserInfoService
    {
        Task<HttpResult<UserInfo>> HandleAsync(String accessToken, CancellationToken cancellationToken = default); 
    }

    internal class GetUserInfoService(IHttpClientFactory _httpClientFactory, AuthorizationServerOptions _authorizationServerOptions) : IGetUserInfoService
    {
        public async Task<HttpResult<UserInfo>> HandleAsync(String accessToken, CancellationToken cancellationToken = default)
        {
            using HttpClient httpClient = _httpClientFactory.CreateClient(_authorizationServerOptions.ClientName);

            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, _authorizationServerOptions.UserInfoEndpoint);

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var userInfoResponse = await httpClient.SendAsync(requestMessage, cancellationToken);
            if (userInfoResponse.IsSuccessStatusCode)
            {
                var userInfo = await userInfoResponse.Content.ReadFromJsonAsync<UserInfo>(cancellationToken);
                return HttpResult<UserInfo>.Success(userInfo!);
            }

            if (userInfoResponse.IsContentTypeJson())
            {
                var bodyErrorResponse = await userInfoResponse.Content.ReadFromJsonAsync<OAuthError>(cancellationToken);
                if (bodyErrorResponse is not null)
                    return HttpResult<UserInfo>.Failure(bodyErrorResponse);
            }
            
            return HttpResult<UserInfo>.Failure(this.ParseWwwAuthenticateError(userInfoResponse.Headers.WwwAuthenticate)!);
        }

        private OAuthError? ParseWwwAuthenticateError(HttpHeaderValueCollection<AuthenticationHeaderValue> headers)
        {
            foreach (var header in headers)
            {
                if (!String.Equals(header.Scheme, "Bearer", StringComparison.OrdinalIgnoreCase))
                    continue;

                var parameter = header.Parameter;
                if (String.IsNullOrEmpty(parameter))
                    continue;

                var parameters = this.ParseParameters(parameter);

                if (parameters.TryGetValue("error", out var error))
                {
                    parameters.TryGetValue("error_description", out var errorDescription);

                    return new OAuthError() { Error = error, ErrorDescription = errorDescription };
                }
            }

            return null;
        }

        private Dictionary<String, String> ParseParameters(String parameterString)
        {
            var result = new Dictionary<String, String>(StringComparer.OrdinalIgnoreCase);

            var parts = parameterString.Split(',');
            foreach (var part in parts)
            {
                var equalIndex = part.IndexOf('=');
                if (equalIndex > 0)
                {
                    var key = part[..equalIndex].Trim();
                    var value = part[(equalIndex + 1)..].Trim().Trim('"');
                    result[key] = value;
                }
            }

            return result;
        }

    }
}
