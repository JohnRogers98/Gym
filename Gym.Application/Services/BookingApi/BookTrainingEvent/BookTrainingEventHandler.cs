using Gym.Application.Extensions;
using Gym.Application.Services.DomainEventPublisher;
using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain._Shared.Services;
using Gym.Domain.BookingAggregate;
using Gym.Domain.CalendarEventAggregate;
using Gym.Domain.ClientAggregate;
using MediatR;

namespace Gym.Application.Services.BookingApi.BookTrainingEvent
{
    internal class BookTrainingEventHandler(
        ITrainingBookingService _trainingBookingService,
        ICalendarEventQueryService _calendarEventQueryService,
        ICalendarEventRepository _calendarEventRepository,
        IClientQueryService _clientQueryService,
        IClientRepository _clientRepository,
        IBookingRepository _bookingRepository,
        IDomainEventPublisher _domainEventPublisher,
        IUnitOfWork _unitOfWork,
        IExclusiveAccessCoordinator _exclusiveAccessCoordinator) : IRequestHandler<BookTrainingEventCommand, BookingDetails>
    {
        public async Task<BookingDetails> Handle(BookTrainingEventCommand request, CancellationToken cancellationToken)
        {
            Client? client = await _clientQueryService.GetByUserIdAsync(UserId.From(request.userId), cancellationToken);
            CalendarEvent? calendarEvent = await _calendarEventQueryService.GetByIdAsync(CalendarEventId.From(request.calendarEventId), cancellationToken);

            if (calendarEvent is null || client is null)
            {
                throw new ArgumentException("Argument ids not exist.");
            }

            ExclusiveAccessResult exclusiveAccessResult  = await _exclusiveAccessCoordinator
                .TryAcquireAsync(calendarEvent.Id.Value, nameof(BookTrainingEventHandler), cancellationToken);
            if (exclusiveAccessResult.Result is false) 
            {
                throw new Exception("Resource is under lock.");
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                Result<Booking> bookingResult = _trainingBookingService.MakeEventBooking(calendarEvent, client);
                if (bookingResult.Success is false)
                {
                    await _unitOfWork.RollbackAsync(cancellationToken);
                    throw new Exception($"Booking operation failed. {bookingResult.Error!.GetErrorMessage()}");
                }

                await _calendarEventRepository.SaveAsync(calendarEvent, cancellationToken);
                await _clientRepository.SaveAsync(client, cancellationToken);
                await _bookingRepository.SaveAsync(bookingResult.Data!, cancellationToken);

                await _unitOfWork.CommitAsync(cancellationToken);

                await _domainEventPublisher.PublishAsync(bookingResult.Data!.DomainEvents, cancellationToken);

                return bookingResult.Data!.ToDetails();
            }
            finally{
                await _exclusiveAccessCoordinator
                    .ReleaseAsync(calendarEvent.Id.Value, nameof(BookTrainingEventHandler), exclusiveAccessResult.AccessKey!, cancellationToken);
            }
        }
    }
}
