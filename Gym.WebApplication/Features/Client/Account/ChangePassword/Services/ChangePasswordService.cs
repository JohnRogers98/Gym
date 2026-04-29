using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features.Account.ChangePassword.Models.Forms;
using Gym.WebApplication.Features.Client.Account.ChangePassword.Models.Results;
using Gym.WebApplication.Operations;
using Gym.WebDto.Requests.Users;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Account.ChangePassword.Services
{
    public class ChangePasswordService(HttpClient _httpClient) : IRequestHandler<ChangePasswordFormModel, ChangePasswordResult>
    {
        public async Task<AsyncOperation<ChangePasswordResult>> HandleAsync(ChangePasswordFormModel request, CancellationToken cancellationToken = default)
        {
            ChangePasswordRequest changePasswordRequest = new()
            {
                OldPassword = request.OldPassword!,
                NewPassword = request.NewPassword!,
            };

            var response = await _httpClient.PostAsJsonAsync("api/users/actions/change-password", changePasswordRequest, cancellationToken);

            if (response.IsSuccessStatusCode)
                return AsyncOperation<ChangePasswordResult>.Success(new ChangePasswordResult());

            if (response.IsContentTypeProblemDetails())
                return await response.GetFailedOperationFromProblemDetailsAsync<ChangePasswordResult>(cancellationToken);

            return AsyncOperation<ChangePasswordResult>.Failure($"Unknown response type.", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }
}
