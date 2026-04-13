using Gym.WebApplication.Features.Account.ChangePassword.Models.Forms;
using Gym.WebDto.Requests.Users;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Account.ChangePassword.Services
{
    public interface IChangePasswordService
    {
        Task HandleAsync(ChangePasswordFormModel changePasswordFormModel, CancellationToken cancellationToken = default);
    }

    public class ChangePasswordService(HttpClient _httpClient) : IChangePasswordService
    {
        public async Task HandleAsync(ChangePasswordFormModel changePasswordFormModel, CancellationToken cancellationToken = default)
        {
            ChangePasswordRequest changePasswordRequest = new()
            {
                OldPassword = changePasswordFormModel.OldPassword!,
                NewPassword = changePasswordFormModel.NewPassword!,
            };

            await _httpClient.PostAsJsonAsync("api/users/actions/change-password", changePasswordRequest, cancellationToken);
        }
    }
}
