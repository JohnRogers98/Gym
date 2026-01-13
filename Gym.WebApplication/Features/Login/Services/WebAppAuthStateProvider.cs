using Microsoft.AspNetCore.Components.Authorization;

namespace Gym.WebApplication.Features.Login.Services
{
    public class WebAppAuthStateProvider : AuthenticationStateProvider
    {
        private AuthenticationState _authenticationState;

        public WebAppAuthStateProvider(IWebAppAuthService _webAppAuthService)
        {
            _authenticationState = new AuthenticationState(_webAppAuthService.CurrentUser);

            _webAppAuthService.UserChanged += (newUser) =>
            {
                _authenticationState = new AuthenticationState(newUser);
                base.NotifyAuthenticationStateChanged(Task.FromResult(_authenticationState));
            };
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync() => _authenticationState;
    }
}
