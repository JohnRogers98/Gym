using Microsoft.AspNetCore.Components.Authorization;

namespace Gym.WebApplication.Features.Login.Services
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private AuthenticationState _authenticationState;

        public AuthStateProvider(UserAuthState _userAuthState)
        {
            _authenticationState = new AuthenticationState(_userAuthState.CurrentUser);

            _userAuthState.UserChanged += (newUser) =>
            {
                _authenticationState = new AuthenticationState(newUser);
                base.NotifyAuthenticationStateChanged(Task.FromResult(_authenticationState));
            };
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync() => _authenticationState;
    }
}
