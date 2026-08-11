using FluentValidation;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebDto.Requests.Account;
using Gym.WebDto.Responses.Account;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace Gym.WebApplication.RequestHandlers
{
    public class ChargeClientService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions) 
        : IRequestHandler<ChargeClientFormModel, ChargeClientResult>
    {
        public async Task<AsyncOperation<ChargeClientResult>> HandleAsync(ChargeClientFormModel request, CancellationToken cancellationToken = default)
        {
            using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            var endpointUrl = UrlHelper.ReplacePathVariables(_bffOptions.Value.ChargeClientEndpoint, new() { ["clientId"] = request.ClientId! }); 

            using var chargeClientRequest = this.CreatePostRequestWithJson(endpointUrl, new ChargeAccountRequest() { ByCount = request.ByCount });

            var chargeClientResponse = await httpClient.SendAsync(chargeClientRequest, cancellationToken);
            if (chargeClientResponse.IsSuccessStatusCode)
            {
                var deserializedChargeClientResponse = await chargeClientResponse.Content.ReadFromJsonAsync<ChargeAccountResponse>(cancellationToken: cancellationToken);
                if (deserializedChargeClientResponse is null)
                    return AsyncOperation<ChargeClientResult>.EmptyResponseBody();

                return AsyncOperation<ChargeClientResult>.Success(new ChargeClientResult(deserializedChargeClientResponse.AvailableTrainingsCount));
            }

            if (chargeClientResponse.IsContentTypeProblemDetails())
            {
                return await chargeClientResponse.GetFailedOperationFromProblemDetailsAsync<ChargeClientResult>(cancellationToken);
            }

            return AsyncOperation<ChargeClientResult>.UnknownResponseType((Int32)chargeClientResponse.StatusCode);
        }
    }

    public class ChargeClientFormModel
    {
        [Required]
        public String? ClientId { get; set; }

        [Required, Range(1, 100)]
        public Int32 ByCount { get; set; }

        public class Validator : AbstractValidator<ChargeClientFormModel>
        {
            public Validator()
            {
                base.RuleFor(form => form.ClientId)
                    .NotEmpty()
                    .WithMessage("ClientId is required");

                base.RuleFor(form => form.ByCount)
                    .NotEmpty()
                    .WithMessage("Count is required")
                    .GreaterThan(0)
                    .LessThan(100)
                    .WithMessage("Expected range 1..100");
            }
        }
    }

    public record ChargeClientResult(Int32 AvailableTrainingsCount);
}
