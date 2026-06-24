using Microsoft.AspNetCore.Components.Authorization;

namespace Gym.WebApplication.Authentication
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly UserAuthState _userAuthState;
        private readonly ICheckSessionService _checkSessionService;
        private readonly IUserInfoService _userInfoService;

        private Task<AuthenticationState> _authenticationState;

        public AuthStateProvider(
            UserAuthState userAuthState,
            ICheckSessionService checkSessionService,
            IUserInfoService userInfoService)
        {
            (_userAuthState, _checkSessionService, _userInfoService) = (userAuthState, checkSessionService, userInfoService);

            _authenticationState = this.LoadAuthenticationStateAsync();

            userAuthState.UserChanged += (user) => NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return _authenticationState;
        }

        public async Task ReloadAsync()
        {
            var newAuthenticationState = await this.LoadAuthenticationStateAsync();
            _authenticationState = Task.FromResult(newAuthenticationState);
            this.NotifyAuthenticationStateChanged(_authenticationState);
        }

        private async Task<AuthenticationState> LoadAuthenticationStateAsync()
        {
            var checkSessionResult = await _checkSessionService.HandleAsync();
            if (checkSessionResult.Succeeded)
            {
                var userInfoResult = await _userInfoService.HandleAsync();
                if (userInfoResult.Succeeded)
                {
                    AuthClaims authClaims = new()
                    {
                        UserId = userInfoResult.Data!.UserId,
                        Role = userInfoResult.Data.Role,
                        Name = userInfoResult.Data.Name
                    };
                    _userAuthState.CurrentUser = authClaims.ToClaimsPrincipal();
                    return new AuthenticationState(_userAuthState.CurrentUser);
                }
            }

            return new AuthenticationState(new());
        }

    }
}
