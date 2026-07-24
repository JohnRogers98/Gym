using AutoMapper;
using FluentValidation;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebDto.Requests.PersonalTraining;
using Gym.WebDto.Responses.PersonalTraining;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace Gym.WebApplication.RequestHandlers
{
    public class CreatePersonalTrainingService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions, IMapper _mapper) 
        : IRequestHandler<CreatePersonalTrainingFormModel, CreatePersonalTrainingResult>
    {
        public async Task<AsyncOperation<CreatePersonalTrainingResult>> HandleAsync(CreatePersonalTrainingFormModel request, CancellationToken cancellationToken = default)
        {
            using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            var createPersonalTrainingRequestObj = _mapper.Map<CreatePersonalTrainingRequest>(request);
            using var createPersonalTrainingRequest = this.CreatePostRequestWithJson(_bffOptions.Value.CreatePersonalTrainingEndpoint, createPersonalTrainingRequestObj);

            var createPersonalTrainingResponse = await httpClient.SendAsync(createPersonalTrainingRequest, cancellationToken);
            if (createPersonalTrainingResponse.IsSuccessStatusCode)
            {
                var deserializedResponse = await createPersonalTrainingResponse.Content.ReadFromJsonAsync<CreatePersonalTrainingResponse>(cancellationToken: cancellationToken);
                if (deserializedResponse is null)
                    return AsyncOperation<CreatePersonalTrainingResult>.EmptyResponseBody();

                return AsyncOperation<CreatePersonalTrainingResult>.Success(_mapper.Map<CreatePersonalTrainingResult>(deserializedResponse));
            }

            if (createPersonalTrainingResponse.IsContentTypeProblemDetails())
            {
                return await createPersonalTrainingResponse.GetFailedOperationFromProblemDetailsAsync<CreatePersonalTrainingResult>(cancellationToken);
            }

            return AsyncOperation<CreatePersonalTrainingResult>.UnknownResponseType((Int32)createPersonalTrainingResponse.StatusCode);
        }
    }

    public class CreatePersonalTrainingFormModel
    {
        [Required]
        public String? ClientId { get; set; }

        [Required]
        public DateTime? LocalStartDateTime { get; set; }

        [Required]
        public TimeSpan? StartTimeSpan { get; set; }

        public Int32? DurationInMinutes { get; set; }

        public DateTime? LocalStart => LocalStartDateTime + StartTimeSpan;
        public DateTime? UtcStart => LocalStart?.ToUniversalTime();

        public DateTime? LocalEnd => DurationInMinutes.HasValue
            ? LocalStart + TimeSpan.FromMinutes(DurationInMinutes.Value)
            : null;
        public DateTime? UtcEnd => LocalEnd?.ToUniversalTime();

        public Boolean IsPaid { get; set; }

        public String? InstructorComment { get; set; }

        public class Validator : AbstractValidator<CreatePersonalTrainingFormModel>
        {
            public Validator()
            {
                base.RuleFor(form => form.ClientId)
                     .NotEmpty()
                     .WithMessage("ClientId is required");

                base.RuleFor(form => form.LocalStartDateTime)
                    .NotEmpty()
                    .WithMessage("Start date is required")
                    .Must(localStart => localStart >= DateTime.Today)
                    .WithMessage("Must be the future");

                base.RuleFor(form => form.StartTimeSpan)
                     .NotEmpty()
                     .WithMessage("TrainingId is required");
            }
        }
    }

    public record CreatePersonalTrainingResult(String PersonalTrainingId);
}
