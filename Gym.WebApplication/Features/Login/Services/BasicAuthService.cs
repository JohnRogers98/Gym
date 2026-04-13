using Gym.WebApplication.Features.Login.FormAuth.Models.Forms;
using Gym.WebApplication.JSAdapters;
using Gym.WebDto.Responses.Users;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Gym.WebApplication.Features.Login.Services
{
    public interface IBasicAuthService
    {
        Task HandleAsync(AuthFormModel authFormModel, CancellationToken cancellationToken = default);
    }

    public class BasicAuthService(UserAuthState _userAuthState, HttpClient _httpClient, LocalStorageAdapter _localStorage) : IBasicAuthService
    {
        public async Task HandleAsync(AuthFormModel authFormModel, CancellationToken cancellationToken = default)
        {
            String credentials = this.GetCredentialsInBase64(authFormModel.Login!, authFormModel.Password!);

            using var request = new HttpRequestMessage(HttpMethod.Post, "api/users/actions/form-auth");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new IOException($"{nameof(BasicAuthService)} returned {response.StatusCode}");

            AuthResponse authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>()
                ?? throw new IOException($"{nameof(BasicAuthService)} response has no body");

            StoredAuthClaims storedAuthClaims = new() { UserId = authResponse.UserId, Role = authResponse.Role };
            await _localStorage.SetItemAsync("auth-claims", storedAuthClaims);

            _userAuthState.CurrentUser = storedAuthClaims.ToClaimsPrincipal();
        }

        private String GetCredentialsInBase64(String login, String password) 
            => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{login}:{password}"));
    }
}
