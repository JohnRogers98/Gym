namespace Gym.WebApplication.Features.Login.Services
{
    public interface IWebAppAuthService
    {
        Task Authenticate(String initData);
    }
}
