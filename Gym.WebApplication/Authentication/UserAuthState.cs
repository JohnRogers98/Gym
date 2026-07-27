using System.Security.Claims;

namespace Gym.WebApplication.Authentication
{
    public class UserAuthState
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
    }
}
