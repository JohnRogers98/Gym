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
using MediatR;

namespace Gym.Application.Services.BookingApi.BookTrainingEvent
{
    internal class BookTrainingEventHandler(
        ITrainingBookingService _trainingBookingService,
        ICalendarEventRepository _calendarEventRepository,
        IClientByUserIdFinder _clientByUserIdFinder,
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

            await _calendarEventRepository.SaveAsync(calendarEvent!, cancellationToken);
            await _accountRepository.SaveAsync(account, cancellationToken);

            return Result<BookTrainingEventResult>.Ok(new BookTrainingEventResult(bookingResult.Data!.Id.Value));
        }
    }
}
