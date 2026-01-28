using Gym.WebApplication.JSAdapters;
using Gym.WebDto.Requests.Users;
using Gym.WebDto.Responses.Users;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Gym.WebApplication.Features.Login.Services
{
    public interface IWebAppAuthService
    {
        event Action<ClaimsPrincipal>? UserChanged;
        ClaimsPrincipal CurrentUser { get; set; }
        Task Authenticate(String initData);
    }

    public class WebAppAuthService : IWebAppAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageAdapter _localStorage;

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

        public WebAppAuthService(HttpClient httpClient, LocalStorageAdapter localStorage)
        {
            (_httpClient, _localStorage) = (httpClient, localStorage);

            Task.Run(async () =>
            {
                StoredAuthClaims? storedAuthClaims = await _localStorage.GetItemAsync<StoredAuthClaims>("auth-claims");
                if(storedAuthClaims is not null)
                {
                    CurrentUser = storedAuthClaims.ToClaimsPrincipal();
                }
            });
        }

        public async Task Authenticate(String initData)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users/actions/web-app-auth", new WebAppAuthRequest { InitData = initData });

            if (!response.IsSuccessStatusCode)
                throw new IOException($"{nameof(WebAppAuthService)} returned {response.StatusCode}");

            WebAppAuthResponse webAuthResponse = await response.Content.ReadFromJsonAsync<WebAppAuthResponse>() 
                ?? throw new IOException($"{nameof(WebAppAuthService)} response has no body");

            StoredAuthClaims storedAuthClaims = new() { Id = webAuthResponse.Id, Role = webAuthResponse.Role };

            await _localStorage.SetItemAsync("auth-claims", storedAuthClaims);

            CurrentUser = storedAuthClaims.ToClaimsPrincipal();
        }

    }
}
