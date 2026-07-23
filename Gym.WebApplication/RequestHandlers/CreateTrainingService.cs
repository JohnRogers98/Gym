using FluentValidation;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebDto.Requests.Training;
using Gym.WebDto.Responses.Training;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace Gym.WebApplication.RequestHandlers
{
    public class CreateTrainingService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions) 
        : IRequestHandler<CreateTrainingFormModel, CreateTrainingResult>
    {
        public async Task<AsyncOperation<CreateTrainingResult>> HandleAsync(CreateTrainingFormModel request, CancellationToken cancellationToken)
        {
            using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            CreateTrainingRequest createTrainingRequestObj = new()
            {
                Name = request.Name!,
                Description = request.Description
            };
            using HttpRequestMessage createTrainingRequest = this.CreatePostRequestWithJson(_bffOptions.Value.CreateTrainingEndpoint, createTrainingRequestObj);

            var response = await httpClient.SendAsync(createTrainingRequest, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var deserializedResponse = await response.Content.ReadFromJsonAsync<CreateTrainingResponse>(cancellationToken: cancellationToken);
                if (deserializedResponse is null)
                    return AsyncOperation<CreateTrainingResult>.EmptyResponseBody();

                var createTrainingResult = new CreateTrainingResult(deserializedResponse.TrainingId);
                return AsyncOperation<CreateTrainingResult>.Success(createTrainingResult);
            }

            if (response.IsContentTypeProblemDetails())
            {
                return await response.GetFailedOperationFromProblemDetailsAsync<CreateTrainingResult>(cancellationToken);
            }

            return AsyncOperation<CreateTrainingResult>.UnknownResponseType((Int32)response.StatusCode);
        }
    }

    public class CreateTrainingFormModel
    {
        [Required]
        public String? Name { get; set; }

        public String? Description { get; set; }

        public class Validator : AbstractValidator<CreateTrainingFormModel>
        {
            public Validator()
            {
                base.RuleFor(form => form.Name)
                    .NotEmpty()
                    .WithMessage("Name is required");
            }
        }
    }

    public record CreateTrainingResult(String TrainingId);
}
