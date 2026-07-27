using FluentValidation;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebDto.Requests.Users;
using Gym.WebDto.Responses.Users;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace Gym.WebApplication.RequestHandlers
{
    public class CreateInstructorService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions) 
        : IRequestHandler<CreateInstructorFormModel, CreateInstructorResult>
    {
        public async Task<AsyncOperation<CreateInstructorResult>> HandleAsync(CreateInstructorFormModel request, CancellationToken cancellationToken = default)
        {
            using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            CreateUserRequest createUser = new()
            {
                Username = request.Username!,
                Password = request.Password!,
                FirstName = request.FirstName!,
                LastName = request.LastName!,
                RoleId = request.RoleId!
            };
            using HttpRequestMessage createInstructorRequest = this.CreatePostRequestWithJson(_bffOptions.Value.CreateUserEndpoint, createUser);

            var response = await httpClient.SendAsync(createInstructorRequest, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var deserializedResponse = await response.Content.ReadFromJsonAsync<CreateUserResponse>(cancellationToken: cancellationToken);
                if (deserializedResponse is null)
                    return AsyncOperation<CreateInstructorResult>.EmptyResponseBody();

                var createInstructorResult = new CreateInstructorResult()
                {
                    UserId = deserializedResponse.UserId
                };
                return AsyncOperation<CreateInstructorResult>.Success(createInstructorResult);
            }

            if (response.IsContentTypeProblemDetails())
            {
                return await response.GetFailedOperationFromProblemDetailsAsync<CreateInstructorResult>(cancellationToken);
            }

            return AsyncOperation<CreateInstructorResult>.UnknownResponseType((Int32)response.StatusCode);
        }
    }

    public class CreateInstructorFormModel
    {
        [Required]
        public String? Username { get; set; }

        [Required]
        public String? Password { get; set; }

        [Required]
        public String? RoleId { get; set; }

        [Required]
        public String? FirstName { get; set; }

        [Required]
        public String? LastName { get; set; }

        public class Validator : AbstractValidator<CreateInstructorFormModel>
        {
            public Validator(IRequestHandler<CheckUsernameExistence, CheckUsernameExistenceResult> checkUsernameService)
            {
                base.RuleFor(x => x.Username)
                    .NotEmpty()
                    .WithMessage("Username is required")
                    .MustAsync(async (username, cancellation) =>
                    {
                        var result = await checkUsernameService.HandleAsync(
                            new CheckUsernameExistence(username!), cancellation);
                        return result.Succeeded && !result.Data.IsExist;
                    })
                    .WithMessage("Username is already taken");

                base.RuleFor(form => form.Password)
                    .NotEmpty()
                    .WithMessage("Password is required");

                base.RuleFor(form => form.RoleId)
                    .NotEmpty()
                    .WithMessage("RoleId is required");

                base.RuleFor(form => form.FirstName)
                    .NotEmpty()
                    .WithMessage("FirstName is required");

                base.RuleFor(form => form.LastName)
                    .NotEmpty()
                    .WithMessage("FirstName is required");
            }
        }
    }

    public record CreateInstructorResult 
    {
        public required String UserId { get; init; }
    }
}
