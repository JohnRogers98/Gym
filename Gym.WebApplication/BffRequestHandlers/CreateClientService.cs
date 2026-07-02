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

namespace Gym.WebApplication.BffRequestHandlers;

public class CreateClientService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions) 
    : IRequestHandler<CreateClientFormModel, CreateClientResult>
{
    public async Task<AsyncOperation<CreateClientResult>> HandleAsync(CreateClientFormModel request, CancellationToken cancellationToken = default)
    {
        using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

        CreateUserRequest createUser = new()
        {
            Username = request.Username,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName,
            RoleId = request.RoleId
        };
        using HttpRequestMessage createClientRequest = this.CreatePostRequestWithJson(_bffOptions.Value.CreateUserEndpoint, createUser);

        var response = await httpClient.SendAsync(createClientRequest, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var createClientResponse = await response.Content.ReadFromJsonAsync<CreateUserResponse>(cancellationToken: cancellationToken);
            if (createClientResponse is null)
                return AsyncOperation<CreateClientResult>.EmptyResponseBody();

            var createClientResult = new CreateClientResult()
            {
                UserId = createClientResponse.UserId
            };

            return AsyncOperation<CreateClientResult>.Success(createClientResult);
        }

        if(response.IsContentTypeProblemDetails())
        {
            return await response.GetFailedOperationFromProblemDetailsAsync<CreateClientResult>(cancellationToken);
        }

        return AsyncOperation<CreateClientResult>.UnknownResponseType((Int32)response.StatusCode);
    }
}

public class CreateClientFormModel
{
    [Required]
    public String? Username { get; set; }

    [Required]
    public String? Password { get; set; }

    [Required]
    public String? RoleId { get; set; }

    [Required]
    public String? FirstName { get; set; }

    public String? LastName { get; set; }

    public class Validator : AbstractValidator<CreateClientFormModel>
    {
        public Validator(IRequestHandler<CheckUsernameExistence, CheckUsernameExistenceResult> checkUsernameService)
        {
            base.RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username is required")
                .MustAsync(async (username, cancellation) =>
                {
                    var result = await checkUsernameService.HandleAsync(
                        new CheckUsernameExistence(username), cancellation);
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
        }
    }
}

public record CreateClientResult
{
    public required String UserId { get; init; }
}
