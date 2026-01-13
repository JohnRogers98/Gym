using System.Security.Claims;

namespace Gym.WebApplication.Features.Login.Services
{
    public interface IWebAppAuthService
    {
        event Action<ClaimsPrincipal>? UserChanged;
        ClaimsPrincipal CurrentUser { get; set; }
        Task Authenticate(String initData);
    }
}
