using Gym.WebApplication.JSAdapters;
using Gym.WebDto.Requests.Users;
using Gym.WebDto.Responses.Users;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Login.Services
{
    public interface IWebAppAuthService
    {
        Task HandleAsync(String initData);
    }

    public class WebAppAuthService(UserAuthState _userAuthState, HttpClient _httpClient, LocalStorageAdapter _localStorage) : IWebAppAuthService
    {
        public async Task HandleAsync(String initData)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users/actions/web-app-auth", new WebAppAuthRequest { InitData = initData });

            if (!response.IsSuccessStatusCode)
                throw new IOException($"{nameof(WebAppAuthService)} returned {response.StatusCode}");

            AuthResponse webAuthResponse = await response.Content.ReadFromJsonAsync<AuthResponse>() 
                ?? throw new IOException($"{nameof(WebAppAuthService)} response has no body");

            StoredAuthClaims storedAuthClaims = new() { UserId = webAuthResponse.UserId, Role = webAuthResponse.Role };
            await _localStorage.SetItemAsync("auth-claims", storedAuthClaims);

            _userAuthState.CurrentUser = storedAuthClaims.ToClaimsPrincipal();
        }
    }
}
