using Gym.WebApplication.JSAdapters;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Login.Services
{
    public interface IMockedAdminAuthService
    {
        Task HandleAsync();
    }

    public class MockedAdminAuthService(UserAuthState _userAuthState, HttpClient _httpClient, LocalStorageAdapter _localStorage) : IMockedAdminAuthService
    {
        public async Task HandleAsync()
        {
            var response = await _httpClient.PostAsJsonAsync("api/users/actions/admin-auth-mock", new Object());

            if (!response.IsSuccessStatusCode)
                throw new IOException($"{nameof(MockedAdminAuthService)} returned {response.StatusCode}");

            StoredAuthClaims storedAuthClaims = new() { UserId = "Undefined", Role = "Admin" };
            await _localStorage.SetItemAsync("auth-claims", storedAuthClaims);

            _userAuthState.CurrentUser = storedAuthClaims.ToClaimsPrincipal();
        }
    }
}
