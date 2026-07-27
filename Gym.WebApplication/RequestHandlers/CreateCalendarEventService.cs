using AutoMapper;
using FluentValidation;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebDto.Requests.CalendarEvent;
using Gym.WebDto.Responses.CalendarEvent;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace Gym.WebApplication.RequestHandlers
{
    public class CreateCalendarEventService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions, IMapper _mapper) 
        : IRequestHandler<CreateCalendarEventFormModel, CreateCalendarEventResult>
    {
        public async Task<AsyncOperation<CreateCalendarEventResult>> HandleAsync(CreateCalendarEventFormModel request, CancellationToken cancellationToken = default)
        {
            using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            var createCalendarEventRequestObj = _mapper.Map<CreateCalendarEventRequest>(request);
            using var createCalendarEventRequest = this.CreatePostRequestWithJson(_bffOptions.Value.CreateCalendarEventEndpoint, createCalendarEventRequestObj);

            var createCalendarEventResponse = await httpClient.SendAsync(createCalendarEventRequest, cancellationToken);
            if (createCalendarEventResponse.IsSuccessStatusCode)
            {
                var deserializedResponse = await createCalendarEventResponse.Content.ReadFromJsonAsync<CreateCalendarEventResponse>(cancellationToken: cancellationToken);
                if (deserializedResponse is null)
                    return AsyncOperation<CreateCalendarEventResult>.EmptyResponseBody();

                return AsyncOperation<CreateCalendarEventResult>.Success(_mapper.Map<CreateCalendarEventResult>(deserializedResponse));
            }

            if (createCalendarEventResponse.IsContentTypeProblemDetails())
            {
                return await createCalendarEventResponse.GetFailedOperationFromProblemDetailsAsync<CreateCalendarEventResult>(cancellationToken);
            }

            return AsyncOperation<CreateCalendarEventResult>.UnknownResponseType((Int32)createCalendarEventResponse.StatusCode);
        }
    }

    public class CreateCalendarEventFormModel
    {
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

        [Required]
        public String? TrainingId { get; set; }

        public Int32? MaxClientCount { get; set; }

        public IReadOnlyCollection<String> Instructors { get; set; } = [];

        public CreatePollFormModel? PollFormModel { get; set; }

        public class Validator : AbstractValidator<CreateCalendarEventFormModel>
        {
            public Validator()
            {
                base.RuleFor(form => form.LocalStartDateTime)
                    .NotEmpty()
                    .WithMessage("Start date is required")
                    .Must(localStart => localStart >= DateTime.Today)
                    .WithMessage("Must be the future");

                base.RuleFor(form => form.StartTimeSpan)
                     .NotEmpty()
                     .WithMessage("Start time is required");

                base.RuleFor(form => form.TrainingId)
                     .NotEmpty()
                     .WithMessage("TrainingId is required");
                
                base.When(form => form.PollFormModel is not null, () =>
                {
                    base.RuleFor(form => form.PollFormModel)
                        .SetValidator(new CreatePollFormModel.Validator()!);
                });
            }
        }
    }

    public class CreatePollFormModel
    {
        [Required]
        public String? Title { get; set; }

        public Boolean IsRequired { get; set; }

        public Boolean CanSelectMany { get; set; }

        public List<String> Choices { get; set; } = [];

        public class Validator : AbstractValidator<CreatePollFormModel>
        {
            public Validator()
            {
                base.RuleFor(form => form.Title)
                    .NotEmpty()
                    .WithMessage("Title is required");

                base.RuleFor(form => form.Choices)
                    .Must(choices => choices.Any())
                    .WithMessage("Must be at least one choice");
            }
        }
    }

    public record CreateCalendarEventResult(String CalendarEventId);
}
