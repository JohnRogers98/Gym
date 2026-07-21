using FluentValidation;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebDto.Requests.Users;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Gym.WebApplication.RequestHandlers
{
    public class ChangePasswordService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions) 
        : IRequestHandler<ChangePasswordFormModel, ChangePasswordResult>
    {
        public async Task<AsyncOperation<ChangePasswordResult>> HandleAsync(ChangePasswordFormModel request, CancellationToken cancellationToken = default)
        {
            ChangePasswordRequest changePasswordRequestObj = new()
            {
                CurrentPassword = request.CurrentPassword!,
                NewPassword = request.NewPassword!,
            };

            using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            using var changePasswordRequest = this.CreatePostRequestWithJson(_bffOptions.Value.ChangePasswordEndpoint, changePasswordRequestObj);

            var changePasswordResponse = await httpClient.SendAsync(changePasswordRequest, cancellationToken);
            if (changePasswordResponse.IsSuccessStatusCode)
            {
                return AsyncOperation<ChangePasswordResult>.Success(new ChangePasswordResult());
            }

            if (changePasswordResponse.IsContentTypeProblemDetails())
            {
                return await changePasswordResponse.GetFailedOperationFromProblemDetailsAsync<ChangePasswordResult>(cancellationToken);
            }

            return AsyncOperation<ChangePasswordResult>.UnknownResponseType((Int32)changePasswordResponse.StatusCode);
        }
    }

    public class ChangePasswordFormModel
    {
        [Required]
        public String? CurrentPassword { get; set; }

        [Required]
        public String? NewPassword { get; set; }

        public class Validator : AbstractValidator<ChangePasswordFormModel>
        {
            public Validator()
            {
                base.RuleFor(form => form.CurrentPassword)
                    .NotEmpty()
                    .WithMessage("Current password is required");

                base.RuleFor(form => form.NewPassword)
                    .NotEmpty()
                    .WithMessage("New password is required");
            }
        }
    }

    public class ChangePasswordResult;
}
