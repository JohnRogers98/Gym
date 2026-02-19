using Gym.Domain._Exceptions;
using Gym.Domain._Shared;
using Gym.Domain._Shared.Services;
using Gym.Domain.AccountContext;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.ClientContext;
using MediatR;

namespace Gym.Application.Services.BookingApi.BookTrainingEvent
{
    internal class BookTrainingEventHandler(
        ITrainingBookingService _trainingBookingService,
        ICalendarEventQueryService _calendarEventQueryService,
        ICalendarEventRepository _calendarEventRepository,
        IClientQueryService _clientQueryService,
        IAccountRepository _accountRepository) : IRequestHandler<BookTrainingEvent, BookingDetails>
    {
        public async Task<BookingDetails> Handle(BookTrainingEvent request, CancellationToken cancellationToken)
        {
            if (!await _clientQueryService.ExistsByUserIdAsync(UserId.From(request.UserId), cancellationToken) 
                || !await _calendarEventQueryService.ExistsAsync(CalendarEventId.From(request.CalendarEventId), cancellationToken))
            {
                throw new ArgumentException("Argument ids not exist.");
            }

            try
            {
                Client? client = await _clientQueryService.GetByUserIdAsync(UserId.From(request.UserId), cancellationToken);
                if (client == null)
                {
                    throw new ArgumentException($"User - {request.UserId} is not client");
                }

                AccountId accountId = AccountId.From(UserId.From(request.UserId));
                Account account = await _accountRepository.GetByIdAsync(accountId, cancellationToken);
                CalendarEvent? calendarEvent = await _calendarEventQueryService.GetByIdAsync(CalendarEventId.From(request.CalendarEventId), cancellationToken);

                Booking booking = _trainingBookingService.MakeEventBooking(account, calendarEvent!);

                await _calendarEventRepository.SaveAsync(calendarEvent!, cancellationToken);
                await _accountRepository.SaveAsync(account, cancellationToken);

                return booking.ToDetails();
            }
            catch (DomainException domainException)
            {
                throw new Exception($"Booking operation failed. {domainException.Error!.GetErrorMessage()}");
            }
        }
    }
}
