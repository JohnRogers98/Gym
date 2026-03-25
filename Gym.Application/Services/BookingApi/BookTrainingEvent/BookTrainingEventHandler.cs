using Gym.Application.Extensions;
using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain._Shared.Services;
using Gym.Domain.AccountContext;
using Gym.Domain.AccountContext.ValueObjects;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.CalendarEventContext.Errors;
using Gym.Domain.CalendarEventContext.ValueObjects;
using Gym.Domain.ClientContext;
using Gym.Domain.ClientContext.Errors;
using Gym.Domain.InstructorContext.Errors;
using Gym.Domain.PollContext;
using Gym.Domain.PollContext.Errors;
using Gym.Domain.PollContext.ValueObjects;
using Gym.Domain.PollResponseContext;
using Gym.Domain.PollResponseContext.Errors;
using MediatR;

namespace Gym.Application.Services.BookingApi.BookTrainingEvent
{
    internal class BookTrainingEventHandler(
        ITrainingBookingService _trainingBookingService,
        ISubmitPollResponseService _submitPollResponseService,
        ICalendarEventRepository _calendarEventRepository,
        IClientByUserIdFinder _clientByUserIdFinder,
        IPollRepository _pollRepository,
        IPollResponseRepository _pollResponseRepository,
        IAccountRepository _accountRepository) : IRequestHandler<BookTrainingEvent, Result<BookTrainingEventResult>>
    {
        public async Task<Result<BookTrainingEventResult>> Handle(BookTrainingEvent request, CancellationToken cancellationToken)
        {
            var userIdResult = UserId.From(request.UserId);
            if (userIdResult.Success is false)
                return Result<BookTrainingEventResult>.Fail(userIdResult.Error!);

            var calendarEventIdResult = CalendarEventId.From(request.CalendarEventId);
            if (calendarEventIdResult.Success is false)
                return Result<BookTrainingEventResult>.Fail(calendarEventIdResult.Error!);

            Client? client = await _clientByUserIdFinder.GetByUserIdAsync(userIdResult.Data!, cancellationToken);
            if (client is null)
                return Result<BookTrainingEventResult>.Fail(ClientNotFoundByUserIdError.Create(userIdResult.Data!));

            CalendarEvent? calendarEvent = await _calendarEventRepository.GetByIdAsync(calendarEventIdResult.Data!, cancellationToken);
            if(calendarEvent is null)
                return Result<BookTrainingEventResult>.Fail(CalendarEventNotFoundError.Create(calendarEventIdResult.Data!));

            AccountId accountId = AccountId.From(userIdResult.Data!);
            Account account = await _accountRepository.GetByIdAsync(accountId, cancellationToken);

            var bookingResult = _trainingBookingService.MakeEventBooking(account, calendarEvent);
            if(bookingResult.Success is false)
                return Result<BookTrainingEventResult>.Fail(bookingResult.Error!);

            await _calendarEventRepository.SaveAsync(calendarEvent, cancellationToken);
            await _accountRepository.SaveAsync(account, cancellationToken);

            if (calendarEvent.PollId?.Value != request.PollResponse?.PollId)
                return Result<BookTrainingEventResult>.Fail(PollIdValidationError.Create());

            if(calendarEvent.PollId is not null)
            {    
                Poll? poll = await _pollRepository.GetByIdAsync(calendarEvent.PollId, cancellationToken);
                if(poll is null)
                    return Result<BookTrainingEventResult>.Fail(PollNotFoundError.Create(calendarEvent.PollId));

                var submitPollResponseResult = await this.SubmitPollResponseAsync(poll, request.PollResponse, userIdResult.Data!, cancellationToken);
                if(submitPollResponseResult.Success is false)
                    return Result<BookTrainingEventResult>.Fail(submitPollResponseResult.Error!);
            }

            return Result<BookTrainingEventResult>.Ok(new BookTrainingEventResult(bookingResult.Data!.Id.Value));
        }


        private async Task<Result> SubmitPollResponseAsync(Poll poll, CalendarEventPollResponse? calendarEventPollResponse, UserId userId, CancellationToken cancellationToken)
        {
            if (poll.IsResponseRequired.Value is true && calendarEventPollResponse is null)
                return Result.Fail(PollResponseRequiredError.Create());

            if (calendarEventPollResponse is not null)
            {
                List<ChoiceId> choiceIds = new();
                foreach (var aChoiceId in calendarEventPollResponse.SelectedChoices)
                {
                    var choiceIdResult = ChoiceId.From(aChoiceId);
                    if (choiceIdResult.Success is false)
                        return Result.Fail(choiceIdResult.Error!);

                    choiceIds.Add(choiceIdResult.Data!);
                }

                PollResponse pollResponse = PollResponse.Create(userId, poll.Id, choiceIds);

                var submitPollResponseResult = _submitPollResponseService.Submit(poll, pollResponse);
                if (submitPollResponseResult.Success is false)
                    return Result.Fail(submitPollResponseResult.Error!);

                await _pollResponseRepository.SaveAsync(pollResponse, cancellationToken);
            }

            return Result.Ok();
        }

    }
}
