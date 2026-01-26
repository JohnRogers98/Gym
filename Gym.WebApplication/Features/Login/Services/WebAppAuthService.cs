using Gym.WebDto.Requests.Users;
using Gym.WebDto.Responses.Users;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Gym.WebApplication.Features.Login.Services
{
    public class WebAppAuthService(HttpClient _httpClient) : IWebAppAuthService
    {
        public event Action<ClaimsPrincipal>? UserChanged;

        public ClaimsPrincipal CurrentUser
        {
            get { return field ?? new(); }
            set
            {
                field = value;

                if (UserChanged is not null)
                {
                    UserChanged(field);
                }
            }
        }

        public async Task Authenticate(String initData)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users/actions/web-app-auth", new WebAppAuthRequest(initData));

            if (!response.IsSuccessStatusCode)
                throw new IOException($"{nameof(WebAppAuthService)} returned {response.StatusCode}");

            WebAppAuthResponse webAuthResponse = await response.Content.ReadFromJsonAsync<WebAppAuthResponse>() 
                ?? throw new IOException($"{nameof(WebAppAuthService)} response has no body");

            var identity = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Role, webAuthResponse.role),
            ], "WebApp Authentication");

            CurrentUser = new ClaimsPrincipal(identity);
        }
    }
}
