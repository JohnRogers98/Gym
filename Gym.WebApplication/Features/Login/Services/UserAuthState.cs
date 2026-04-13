using Gym.WebApplication.JSAdapters;
using System.Security.Claims;

namespace Gym.WebApplication.Features.Login.Services
{
    public class UserAuthState
    {
        private LocalStorageAdapter _localStorage;

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

        public UserAuthState(LocalStorageAdapter localStorage)
        {
            _localStorage = localStorage;

            Task.Run(async () =>
            {
                StoredAuthClaims? storedAuthClaims = await _localStorage.GetItemAsync<StoredAuthClaims>("auth-claims");
                if (storedAuthClaims is not null)
                {
                    CurrentUser = storedAuthClaims.ToClaimsPrincipal();
                }
            });
        }

    }
}
