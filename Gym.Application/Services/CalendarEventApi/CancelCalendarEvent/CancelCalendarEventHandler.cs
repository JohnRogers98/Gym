using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain._Shared.Services;
using Gym.Domain.AccountContext;
using Gym.Domain.AccountContext.ValueObjects;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.CalendarEventContext.Errors;
using Gym.Domain.CalendarEventContext.ValueObjects;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.CancelCalendarEvent
{
    internal class CancelCalendarEventHandler(
        ICalendarEventRepository _calendarEventRepository,
        IAccountRepository _accountRepository,
        ICancelCalendarEventService _cancelCalendarEventService) : IRequestHandler<CancelCalendarEvent, Result<CancelCalendarEventResult>>
    {
        public async Task<Result<CancelCalendarEventResult>> Handle(CancelCalendarEvent request, CancellationToken cancellationToken)
        {
            var calendarEventIdResult = CalendarEventId.From(request.CalendarEventId);
            if (calendarEventIdResult.Success is false)
                return Result<CancelCalendarEventResult>.Fail(calendarEventIdResult.Error!);

            CalendarEvent? calendarEvent = await _calendarEventRepository.GetByIdAsync(calendarEventIdResult.Data!, cancellationToken);
            if (calendarEvent is null)
                return Result<CancelCalendarEventResult>.Fail(CalendarEventNotFoundError.Create(calendarEventIdResult.Data!));

            List<Account> accounts = new();
            foreach (UserId bookingUser in calendarEvent.Bookings)
            {
                Account anAccount = await _accountRepository.GetByIdAsync(AccountId.From(bookingUser), cancellationToken);
                accounts.Add(anAccount);
            }

            Result cancelResult = _cancelCalendarEventService.Cancel(calendarEvent, accounts);
            if(cancelResult.Success is false)
                return Result<CancelCalendarEventResult>.Fail(cancelResult.Error!);

            await _calendarEventRepository.SaveAsync(calendarEvent, cancellationToken);

            foreach (Account anAccount in accounts)
            {
                await _accountRepository.SaveAsync(anAccount, cancellationToken);
            }

            return Result<CancelCalendarEventResult>.Ok(new CancelCalendarEventResult());
        }
    }
}
