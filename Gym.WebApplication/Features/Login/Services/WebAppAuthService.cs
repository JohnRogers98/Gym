using Gym.WebDto.Requests.Users;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Login.Services
{
    public class WebAppAuthService(HttpClient _httpClient) : IWebAppAuthService
    {
        public async Task Authenticate(String initData)
        {
            var httpResult = await _httpClient.PostAsJsonAsync("api/users/web-app-auth", new WebAppAuthRequest(initData));
        }
    }
}
