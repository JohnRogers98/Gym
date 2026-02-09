using Gym.Application.Services.DomainEventPublisher;
using Gym.Domain._Common;
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
        IAccountRepository _accountRepository,
        IDomainEventPublisher _domainEventPublisher,
        IUnitOfWork _unitOfWork,
        IExclusiveAccessCoordinator _exclusiveAccessCoordinator) : IRequestHandler<BookTrainingEvent, BookingDetails>
    {
        public async Task<BookingDetails> Handle(BookTrainingEvent request, CancellationToken cancellationToken)
        {
            if (!await _clientQueryService.ExistsByUserIdAsync(UserId.From(request.UserId), cancellationToken) 
                || !await _calendarEventQueryService.ExistsAsync(CalendarEventId.From(request.CalendarEventId), cancellationToken))
            {
                throw new ArgumentException("Argument ids not exist.");
            }

            ExclusiveAccessResult exclusiveAccessResult  = await _exclusiveAccessCoordinator
                .TryAcquireAsync(request.CalendarEventId, nameof(BookTrainingEventHandler), cancellationToken);
            if (exclusiveAccessResult.Result is false) 
            {
                throw new Exception("Resource is under lock.");
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

                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                Booking booking = _trainingBookingService.MakeEventBooking(account, calendarEvent!);

                await _calendarEventRepository.SaveAsync(calendarEvent!, cancellationToken);
                await _accountRepository.SaveAsync(account, cancellationToken);

                await _unitOfWork.CommitAsync(cancellationToken);

                await _domainEventPublisher.PublishAsync(account!.DomainEvents, cancellationToken);
                await _domainEventPublisher.PublishAsync(calendarEvent!.DomainEvents, cancellationToken);

                return booking.ToDetails();
            }
            catch (DomainException domainException)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw new Exception($"Booking operation failed. {domainException.Error!.GetErrorMessage()}");
            }
            catch
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
            finally{
                await _exclusiveAccessCoordinator
                    .ReleaseAsync(request.CalendarEventId, nameof(BookTrainingEventHandler), exclusiveAccessResult.AccessKey!, cancellationToken);
            }
        }
    }
}
