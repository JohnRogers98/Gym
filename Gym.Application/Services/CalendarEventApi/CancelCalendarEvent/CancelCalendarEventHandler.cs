using Gym.Domain._Shared;
using Gym.Domain._Shared.Services;
using Gym.Domain.AccountContext;
using Gym.Domain.CalendarEventContext;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.CancelCalendarEvent
{
    internal class CancelCalendarEventHandler(
        ICalendarEventRepository _calendarEventRepository,
        IAccountRepository _accountRepository,
        ICancelCalendarEventService _cancelCalendarEventService) : IRequestHandler<CancelCalendarEvent, CancelCalendarEventResult>
    {
        public async Task<CancelCalendarEventResult> Handle(CancelCalendarEvent request, CancellationToken cancellationToken)
        {
            CalendarEvent calendarEvent = await _calendarEventRepository.GetByIdAsync(CalendarEventId.From(request.CalendarEventId), cancellationToken)
                ?? throw new ArgumentNullException();

            List<Account> accounts = new();
            foreach (UserId bookingUser in calendarEvent.Bookings)
            {
                Account anAccount = await _accountRepository.GetByIdAsync(AccountId.From(bookingUser), cancellationToken);
                accounts.Add(anAccount);
            }

            _cancelCalendarEventService.Cancel(calendarEvent, accounts);

            await _calendarEventRepository.SaveAsync(calendarEvent, cancellationToken);

            foreach (Account anAccount in accounts)
            {
                await _accountRepository.SaveAsync(anAccount, cancellationToken);
            }

            return new CancelCalendarEventResult();
        }
    }
}
