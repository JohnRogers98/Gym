using Gym.Domain._Exceptions;
using Gym.Domain._Shared;
using Gym.Domain._Shared.Services;
using Gym.Domain.AccountContext;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.ClientContext;
using Gym.Domain.UserContext;
using MediatR;

namespace Gym.Application.Services.BookingApi.BookTrainingEvent
{
    internal class BookTrainingEventHandler(
        ITrainingBookingService _trainingBookingService,
        ICalendarEventRepository _calendarEventRepository,
        IClientByUserIdFinder _clientByUserIdFinder,
        IAccountRepository _accountRepository) : IRequestHandler<BookTrainingEvent, BookTrainingEventResult>
    {
        public async Task<BookTrainingEventResult> Handle(BookTrainingEvent request, CancellationToken cancellationToken)
        {
            Client? client = await _clientByUserIdFinder.GetByUserIdAsync(UserId.From(request.UserId), cancellationToken)
                ?? throw new ArgumentException($"{nameof(User)} - {request.UserId} is not client");

            CalendarEvent? calendarEvent = await _calendarEventRepository.GetByIdAsync(CalendarEventId.From(request.CalendarEventId), cancellationToken)
                ?? throw new ArgumentException($"{nameof(CalendarEvent)} - {request.CalendarEventId} is not exist");

            AccountId accountId = AccountId.From(UserId.From(request.UserId));
            Account account = await _accountRepository.GetByIdAsync(accountId, cancellationToken);

            try
            {
                Booking booking = _trainingBookingService.MakeEventBooking(account, calendarEvent);

                await _calendarEventRepository.SaveAsync(calendarEvent!, cancellationToken);
                await _accountRepository.SaveAsync(account, cancellationToken);

                return new BookTrainingEventResult(booking.Id.Value);
            }
            catch (DomainException domainException)
            {
                throw new Exception($"Booking operation failed. {domainException.Error!.GetErrorMessage()}");
            }
        }
    }
}
